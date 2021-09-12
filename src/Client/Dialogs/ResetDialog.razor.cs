
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace InterviewApp.Client.Dialogs;
public partial class ResetDialog
{
    [CascadingParameter] MudDialogInstance? MudDialog { get; set; }

    [Parameter] public string? Content { get; set; }

    void Submit() 
        => MudDialog?.Close(DialogResult.Ok(true));
    void Cancel() 
        => MudDialog?.Cancel();
}
