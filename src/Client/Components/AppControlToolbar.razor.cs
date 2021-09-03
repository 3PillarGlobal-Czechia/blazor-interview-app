
using Microsoft.AspNetCore.Components;

namespace InterviewApp.Client.Components;

public partial class AppControlToolbar
{
    [Parameter]
    public EventCallback<string> OnFilterList { get; set; }

    [Parameter]
    public EventCallback<object> OnOpenResetAllDialog { get; set; }

    public string? SearchValue { get; set; }

    public async Task FilterList(string? search)
    {
        await OnFilterList.InvokeAsync(search);
    }

    public async Task OpenResetAllDialog()
    {
        await OnOpenResetAllDialog.InvokeAsync();
    }
}
