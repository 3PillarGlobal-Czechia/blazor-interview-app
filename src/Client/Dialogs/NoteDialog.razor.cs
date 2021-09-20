
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace InterviewApp.Client.Dialogs;

public partial class NoteDialog
{
    [CascadingParameter] 
    MudDialogInstance? MudDialog { get; set; }

    [Parameter] 
    public string? Note { get; set; }

    [Parameter] 
    public string? Question { get; set; }

    void Submit()
        => MudDialog?.Close(DialogResult.Ok(Note?.Trim()));
    void Cancel()
        => MudDialog?.Cancel();
}
