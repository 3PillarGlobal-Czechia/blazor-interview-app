
using InterviewApp.Client.Services.Interface;
using Microsoft.JSInterop;

namespace InterviewApp.Client.Services;

public class ClipboardService : IClipboardService
{
    private readonly IJSRuntime _jsRuntime;

    public ClipboardService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public ValueTask WriteTextAsync(string text)
        => _jsRuntime.InvokeVoidAsync("navigator.clipboard.writeText", text);
}
