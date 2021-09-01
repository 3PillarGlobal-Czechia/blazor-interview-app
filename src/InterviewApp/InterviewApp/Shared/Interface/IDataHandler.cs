using InterviewApp.Shared.Models;

namespace InterviewApp.Shared.Interface
{
    public interface IDataHandler
    {
        List<InterviewQuestion> GetDataFromFile();
    }
}