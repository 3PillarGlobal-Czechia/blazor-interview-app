
using InterviewApp.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace InterviewApp.Client.Components.Header;

public partial class AppMainHeader
{
    [Parameter]
    public string? Title { get; set; }

    [Parameter]
    public Func<string, IEnumerable<InterviewQuestion>>? OnSearch { get; set; }

    [Parameter]
    public Func<string, object>? OnSearchText { get; set; }

    [Parameter]
    public Func<InterviewQuestion, object>? OnSearchValue { get; set; }

    [Parameter]
    public EventCallback OnSwitchTheme { get; set; }

    [Parameter]
    public EventCallback OnOpenSettings { get; set; }

    [Parameter]
    public EventCallback OnResetQuestions { get; set; }

    [Parameter]
    public EventCallback OnOpenReport { get; set; }

    public void SearchText(string text)
        => OnSearchText!.Invoke(text);

    public void SearchValue(InterviewQuestion value)
        => OnSearchValue!.Invoke(value);

    public async Task SwitchTheme()
        => await OnSwitchTheme.InvokeAsync();

    public async Task OpenSettings()
        => await OnOpenSettings.InvokeAsync();

    public async Task ResetQuestions()
        => await OnResetQuestions.InvokeAsync();

    public async Task OpenReport()
        => await OnOpenReport.InvokeAsync();

    public async Task<IEnumerable<InterviewQuestion>> Search(string text)
    {
        if (OnSearch is null)
        {
            throw new InvalidOperationException(nameof(OnSearch));
        }

        await Task.Delay(1);

        return OnSearch.Invoke(text);
    }
}
