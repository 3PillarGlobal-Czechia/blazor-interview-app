
using InterviewApp.Client.Enums;
using InterviewApp.Shared.Models;

namespace InterviewApp.Client.Services.Interface;

interface IInterviewService
{
    IDictionary<QuestionListType, IEnumerable<InterviewQuestion>> QuestionLists { get; set; }

    IList<string?> SelectedCategories { get; set; }

    Task Initialize();

    bool IsInitialized();

    void PrepareInterviewQuestions(IList<string?> categories);

    void ResetQuestion(InterviewQuestion? question);

    void ResetQuestions(IList<string?> categories);

    void FilterQuestions(string? search);

    void UpdateQuestion(InterviewQuestion original, InterviewQuestion updated, QuestionListType listType = QuestionListType.Current);

    IList<string?> GetCategories();
}
