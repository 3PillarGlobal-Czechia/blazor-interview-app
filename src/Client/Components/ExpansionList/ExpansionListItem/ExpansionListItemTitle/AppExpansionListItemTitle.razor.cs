
using InterviewApp.Shared.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace InterviewApp.Client.Components.ExpansionList.ExpansionListItem.ExpansionListItemTitle;

public partial class AppExpansionListItemTitle
{
    [Parameter]
    public InterviewQuestion? Question { get; set; }

    [Parameter]
    public EventCallback<InterviewQuestion> OnTogglePin { get; set; }

    [Parameter]
    public EventCallback<InterviewQuestion> OnOpenNoteDialog { get; set; }

    [Parameter]
    public EventCallback<InterviewQuestion> OnOpenResetDialog { get; set; }

    public async Task TogglePin(InterviewQuestion question)
        => await OnTogglePin.InvokeAsync(question);

    public async Task OpenNoteDialog(InterviewQuestion question)
        => await OnOpenNoteDialog.InvokeAsync(question);

    public async Task OpenResetDialog(InterviewQuestion question)
        => await OnOpenResetDialog.InvokeAsync(question);

    public string GetNoteIcon(string? note)
        => string.IsNullOrWhiteSpace(note) ? Icons.Material.Outlined.ChatBubbleOutline : Icons.Material.Outlined.ChatBubble;
}
