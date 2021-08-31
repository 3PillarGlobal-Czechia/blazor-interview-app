using InterviewApp.Shared.Models;

namespace InterviewApp.Shared.Interface
{
    interface IDataHandler
    {
        List<InterviewQuestion> GetDataFromFile();
    }
}