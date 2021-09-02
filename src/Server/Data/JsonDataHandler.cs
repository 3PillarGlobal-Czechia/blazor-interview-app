using System.Reflection;
using InterviewApp.Shared.Models;
using Newtonsoft.Json;

namespace InterviewApp.Server.Data
{
    public class JsonDataHandler : IDataHandler
    {
        private readonly string _jsonFilePath;

        public JsonDataHandler()
        {
            var executablePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            if(executablePath is null)
                throw new ArgumentNullException("Executable path was null");

            _jsonFilePath = Path.Combine(executablePath, "questions.json");
        }

        public List<InterviewQuestion> GetInterviewQuestions()
        {
            var fileContent = File.ReadAllText(_jsonFilePath);

            if (fileContent is null)
                throw new ArgumentNullException(nameof(fileContent));

            var jsonData = JsonConvert.DeserializeObject<List<InterviewQuestion>>(fileContent);

            if (jsonData is null)
                throw new ArgumentNullException(nameof(jsonData));

            return jsonData;
        }
    }
}
