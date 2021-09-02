using InterviewApp.Shared.Models;

namespace InterviewApp.Server.Data
{
    public interface IDataHandler
    {
        List<InterviewQuestion> GetInterviewQuestions();
    }
}
