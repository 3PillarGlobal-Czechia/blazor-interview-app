using System.Reflection;
using InterviewApp.Shared.Models;
using Newtonsoft.Json;

namespace InterviewApp.Server.Data
{
    public class JsonDataHandler : IDataHandler
    {
        private readonly IEnumerable<InterviewQuestion> _interviewQuestions;

        public JsonDataHandler()
        {
            var executablePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            if (executablePath is null)
                throw new ArgumentNullException("Executable path was null");

            var jsonFilePath = Path.Combine(executablePath, "questions.json");

            var fileContent = File.ReadAllText(jsonFilePath);

            if (fileContent is null)
                throw new ArgumentNullException(nameof(fileContent));

            _interviewQuestions = JsonConvert.DeserializeObject<IEnumerable<InterviewQuestion>>(fileContent) ?? Array.Empty<InterviewQuestion>();
        }

        public List<InterviewQuestion> GetInterviewQuestions() => _interviewQuestions.ToList();       
    }
}
