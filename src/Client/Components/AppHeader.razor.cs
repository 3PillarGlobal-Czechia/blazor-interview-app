
using Microsoft.AspNetCore.Components;

namespace InterviewApp.Client.Components;

public partial class AppHeader
{
    [Parameter]
    public string? Title { get; set; }
}
