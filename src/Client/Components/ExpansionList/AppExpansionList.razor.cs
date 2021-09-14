
using InterviewApp.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace InterviewApp.Client.Components.ExpansionList;

public partial class AppExpansionList
{
    [Parameter]
    public string? Title { get; set; }

    [Parameter]
    public ICollection<InterviewQuestion>? InterviewQuestions { get; set; }

    [Parameter]
    public EventCallback<InterviewQuestion> OnTogglePin { get; set; }

    [Parameter]
    public EventCallback<InterviewQuestion> OnOpenNoteDialog { get; set; }

    [Parameter]
    public EventCallback<InterviewQuestion> OnOpenResetDialog { get; set; }

    [Parameter]
    public EventCallback OnRatingChanged { get; set; }

    [Parameter]
    public bool IsSkeleton { get; set; }

    public void RatingChanged()
        => OnRatingChanged.InvokeAsync();

    public IEnumerable<InterviewQuestion> Search(string value)
    {
        if (InterviewQuestions is null)
        {
            throw new InvalidOperationException(nameof(InterviewQuestions));
        }

        return InterviewQuestions.Where(q => q.Contains(value));
    }
}
