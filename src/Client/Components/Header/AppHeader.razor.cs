
using Microsoft.AspNetCore.Components;

namespace InterviewApp.Client.Components.Header;

public partial class AppHeader
{
    [Parameter]
    public string? Title { get; set; }
}
