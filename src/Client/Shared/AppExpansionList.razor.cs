
using InterviewApp.Shared.Models;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InterviewApp.Client.Shared;

public partial class AppExpansionList
{
    [Parameter]
    public List<InterviewQuestion>? InterviewQuestions { get; set; }

    [Parameter]
    public EventCallback<InterviewQuestion> OnTogglePin { get; set; }

    [Parameter]
    public EventCallback<InterviewQuestion> OnOpenNoteDialog { get; set; }

    [Parameter]
    public EventCallback<InterviewQuestion> OnOpenResetDialog { get; set; }

    public async Task TogglePin(InterviewQuestion question)
    {
        await OnTogglePin.InvokeAsync(question);
    }

    public async Task OpenNoteDialog(InterviewQuestion question)
    {
        await OnOpenNoteDialog.InvokeAsync(question);
    }

    public async Task OpenResetDialog(InterviewQuestion question)
    {
        await OnOpenResetDialog.InvokeAsync(question);
    }
}
