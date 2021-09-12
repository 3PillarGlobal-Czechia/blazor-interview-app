
using InterviewApp.Client.Extensions;
using InterviewApp.Client.Services.Interface;
using InterviewApp.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using System.Text;

namespace InterviewApp.Client.Dialogs;

public partial class ReportDialog
{
    [CascadingParameter] MudDialogInstance? MudDialog { get; set; }

    [Parameter] public List<InterviewQuestion>? Questions { get; set; }

    [Inject]
    public IJSRuntime? JSRuntime { get; set; }

    [Inject]
    public IClipboardService? ClipboardService { get; set; }

    [Inject]
    public ISnackbar? Snackbar { get; set; }

    public string? ReportText { get; set; }

    protected override Task OnInitializedAsync()
    {
        if (Questions is not null)
        {
            var titleText = new StringBuilder();
            var contentText = new StringBuilder();

            Questions.ForEach((q) =>
            {
                if (q.Rating > 0 || !string.IsNullOrWhiteSpace(q.Note))
                    contentText.AppendLine(q.ToString());
            });

            var ratedQuestions = Questions.Where(x => x.Rating > 0);
            if (ratedQuestions.Any())
            {
                var averageRating = Math.Round((double)ratedQuestions.Sum(x => x.Rating) / ratedQuestions.Count(), 2);
                var averageDifficulty = Math.Round((double)ratedQuestions.Sum(x => x.Difficulty) / ratedQuestions.Count(), 2);

                titleText.AppendLine($"Based on rated questions ({ratedQuestions.Count()} / 10)");
                titleText.AppendLine($"Average rating: {averageRating} / 5");
                titleText.AppendLine($"Average difficulty: {averageDifficulty} / 5");

                ReportText = titleText + Environment.NewLine + contentText;
            }
        }

        return base.OnInitializedAsync();
    }

    protected async Task CopyToClipboard()
    {
        if (Snackbar is null)
        {
            throw new InvalidOperationException(nameof(Snackbar));
        }

        await ClipboardService!.WriteTextAsync(ReportText ?? string.Empty);

        Snackbar.Add("Report text copied!", Severity.Success);
    }

    protected async Task Download()
        => await JSRuntime!.SaveAsAsync("report.txt", Encoding.UTF8.GetBytes(ReportText!));

    protected void Cancel() 
        => MudDialog?.Cancel();
}
