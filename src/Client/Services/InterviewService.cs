using InterviewApp.Client.Services.Interface;
using InterviewApp.Shared.Models;
using System.Net.Http.Json;

namespace InterviewApp.Client.Services;

public class InterviewService : IInterviewService
{
    private readonly HttpClient _http;

    public InterviewService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<InterviewQuestion>> GetInterviewQuestions()
    {
        var result = await _http.GetFromJsonAsync<List<InterviewQuestion>>("Interview");

        if (result == null)
        {
            throw new ArgumentNullException(nameof(result));
        }

        return result.ToList();
    }
}
