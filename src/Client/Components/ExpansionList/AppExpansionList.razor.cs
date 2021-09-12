
using InterviewApp.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace InterviewApp.Client.Components.ExpansionList;

public partial class AppExpansionList
{
    [Parameter]
    public string? Title { get; set; }

    [Parameter]
    public IList<InterviewQuestion>? InterviewQuestions { get; set; }

    [Parameter]
    public EventCallback<InterviewQuestion> OnTogglePin { get; set; }

    [Parameter]
    public EventCallback<InterviewQuestion> OnOpenNoteDialog { get; set; }

    [Parameter]
    public EventCallback<InterviewQuestion> OnOpenResetDialog { get; set; }
}
