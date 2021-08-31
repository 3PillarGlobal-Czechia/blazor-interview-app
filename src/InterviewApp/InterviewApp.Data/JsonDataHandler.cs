
using InterviewApp.Data.Interface;
using InterviewApp.Shared;
using Newtonsoft.Json;

namespace InterviewApp.Data;

public class JsonDataHandler : IDataHandler
{
    public JsonDataHandler() { }

    public List<InterviewQuestion> GetData()
    {
        var projectDirectory = Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.FullName;

        if (projectDirectory == null)
        {
            throw new ArgumentNullException(nameof(projectDirectory));
        }

        var filePath = Path.Combine(projectDirectory, "data", "questions.json");
        var fileContent = File.ReadAllText(filePath);

        if (fileContent == null)
        {
            throw new ArgumentNullException(nameof(fileContent));
        }

        var jsonData = JsonConvert.DeserializeObject<InterviewQuestion[]>(fileContent);

        if (jsonData == null)
        {
            throw new ArgumentNullException(nameof(jsonData));
        }

        return jsonData.ToList();
    }
}