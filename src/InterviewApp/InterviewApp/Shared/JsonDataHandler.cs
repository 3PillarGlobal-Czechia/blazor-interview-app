using InterviewApp.Shared.Interface;
using InterviewApp.Shared.Models;
using Newtonsoft.Json;

namespace InterviewApp.Shared
{
    public class JsonDataHandler : IDataHandler
    {
        public List<InterviewQuestion> GetDataFromFile()
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
}
