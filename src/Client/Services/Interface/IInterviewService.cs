
using InterviewApp.Client.Enums;
using InterviewApp.Shared.Models;

namespace InterviewApp.Client.Services.Interface;

interface IInterviewService
{
    Dictionary<InterviewQuestionListType, IEnumerable<InterviewQuestion>> InterviewQuestionLists { get; set; }

    Task Initialize();

    bool IsInitialized();

    void PrepareInterviewQuestions();

    void ResetQuestions(InterviewQuestion? question = null);

    void FilterQuestions(string? search);

    void UpdateQuestion(InterviewQuestion original, InterviewQuestion updated, InterviewQuestionListType listType = InterviewQuestionListType.CURRENT);
}
