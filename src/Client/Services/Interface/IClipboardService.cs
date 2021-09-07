
namespace InterviewApp.Client.Services.Interface;

public interface IClipboardService
{
    ValueTask WriteTextAsync(string text);
}
