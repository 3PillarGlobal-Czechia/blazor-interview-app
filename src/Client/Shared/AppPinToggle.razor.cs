
using InterviewApp.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace InterviewApp.Client.Shared;

public partial class AppPinToggle
{
    [Parameter]
    public InterviewQuestion? Question { get; set; }

    [Parameter]
    public EventCallback<InterviewQuestion> OnToggledChanged { get; set; }

    public async Task ToggledChanged(InterviewQuestion question)
    {
        await OnToggledChanged.InvokeAsync(question);
    }
}
