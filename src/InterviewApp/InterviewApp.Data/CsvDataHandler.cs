using InterviewApp.Data.Interface;
using InterviewApp.Shared;

namespace InterviewApp.Data;

public class CsvDataHandler : IDataHandler
{
    public CsvDataHandler() { }

    public List<InterviewQuestion> GetData()
    {
        throw new NotImplementedException(nameof(CsvDataHandler));
    }
}