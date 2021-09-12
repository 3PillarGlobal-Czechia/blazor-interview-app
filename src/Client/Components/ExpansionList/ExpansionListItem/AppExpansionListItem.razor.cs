
using InterviewApp.Shared.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace InterviewApp.Client.Components.ExpansionList.ExpansionListItem;

public partial class AppExpansionListItem
{
    [Parameter]
    public InterviewQuestion? Question { get; set; }

    [Parameter]
    public EventCallback<InterviewQuestion> OnTogglePin { get; set; }

    [Parameter]
    public EventCallback<InterviewQuestion> OnOpenNoteDialog { get; set; }

    [Parameter]
    public EventCallback<InterviewQuestion> OnOpenResetDialog { get; set; }
}
