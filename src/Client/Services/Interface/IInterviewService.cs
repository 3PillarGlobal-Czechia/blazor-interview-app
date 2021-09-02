using InterviewApp.Shared.Models;

namespace InterviewApp.Client.Services.Interface;

interface IInterviewService
{
    Task<List<InterviewQuestion>> GetInterviewQuestions();
}
