
using Microsoft.JSInterop;

namespace InterviewApp.Client.Extensions;

public static class JSRuntimeExtensions
{
    public async static Task SaveAsAsync(this IJSRuntime js, string fileName, byte[] data) 
        => await js.InvokeAsync<object>("saveAsFile", fileName, Convert.ToBase64String(data));
}
