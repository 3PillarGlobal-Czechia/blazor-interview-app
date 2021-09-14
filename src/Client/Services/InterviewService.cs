
using InterviewApp.Client.Enums;
using InterviewApp.Client.Extensions;
using InterviewApp.Client.Services.Interface;
using InterviewApp.Shared.Models;
using System.Net.Http.Json;

namespace InterviewApp.Client.Services;

public class InterviewService : IInterviewService
{
    public Dictionary<InterviewQuestionListType, IEnumerable<InterviewQuestion>> InterviewQuestionLists { get; set; }

    private readonly HttpClient _http;

    public InterviewService(HttpClient http)
    {
        _http = http;

        InterviewQuestionLists = new Dictionary<InterviewQuestionListType, IEnumerable<InterviewQuestion>>();
    }

    public bool IsInitialized()
    {
        return InterviewQuestionLists is not null &&
               InterviewQuestionLists.ContainsKey(InterviewQuestionListType.ALL) &&
               InterviewQuestionLists.ContainsKey(InterviewQuestionListType.CURRENT);
    }

    public async Task Initialize()
    {
        if (!InterviewQuestionLists.ContainsKey(InterviewQuestionListType.ALL))
        {
            var dataset = await _http.GetFromJsonAsync<List<InterviewQuestion>>("Interview");
            if (dataset is not null)
            {
                InterviewQuestionLists.Add(InterviewQuestionListType.ALL, dataset);

                PrepareInterviewQuestions();
            }
        }
    }

    public void PrepareInterviewQuestions()
    {
        if (!InterviewQuestionLists.ContainsKey(InterviewQuestionListType.ALL))
        {
            throw new InvalidOperationException($"{nameof(InterviewQuestionLists)} cannot be null for {nameof(InterviewQuestionListType.ALL)}");
        }

        var current = new List<InterviewQuestion>();
        var previous = new List<InterviewQuestion>();
        var discarded = new List<InterviewQuestion>();

        if (InterviewQuestionLists.ContainsKey(InterviewQuestionListType.CURRENT))
            current = InterviewQuestionLists[InterviewQuestionListType.CURRENT].ToList();

        if (InterviewQuestionLists.ContainsKey(InterviewQuestionListType.PREVIOUS))
            previous = InterviewQuestionLists[InterviewQuestionListType.PREVIOUS].ToList();

        if (InterviewQuestionLists.ContainsKey(InterviewQuestionListType.DISCARDED))
            discarded = InterviewQuestionLists[InterviewQuestionListType.DISCARDED].ToList();

        current = InterviewQuestionLists[InterviewQuestionListType.ALL].Except(current).Except(previous).Except(discarded).Random(10).ToList();
        for (int i = 0; i < current.Count(); i++)
        {
            current[i].Reset(i);
        }

        InterviewQuestionLists.Remove(InterviewQuestionListType.CURRENT);
        InterviewQuestionLists.Remove(InterviewQuestionListType.PREVIOUS);
        InterviewQuestionLists.Remove(InterviewQuestionListType.FILTERED);
        InterviewQuestionLists.Remove(InterviewQuestionListType.DISCARDED);

        InterviewQuestionLists.Add(InterviewQuestionListType.CURRENT, current.AsEnumerable());
        InterviewQuestionLists.Add(InterviewQuestionListType.PREVIOUS, previous.AsEnumerable());
        InterviewQuestionLists.Add(InterviewQuestionListType.FILTERED, current.AsEnumerable());
        InterviewQuestionLists.Add(InterviewQuestionListType.DISCARDED, discarded.AsEnumerable());
    }

    public void ResetQuestions(InterviewQuestion? question = null)
    {
        if (!InterviewQuestionLists.ContainsKey(InterviewQuestionListType.CURRENT))
        {
            throw new InvalidOperationException($"{nameof(InterviewQuestionLists)} cannot be null for {nameof(InterviewQuestionListType.CURRENT)}");
        }

        if (question is null)
        {
            PrepareInterviewQuestions();

            return;
        }

        if (string.IsNullOrWhiteSpace(question.Title))
        {
            throw new InvalidOperationException($"{nameof(question.Title)} cannot be null or empty.");
        }

        var currentList = InterviewQuestionLists[InterviewQuestionListType.CURRENT].ToList();
        currentList[currentList.IndexOf(question)] =
            InterviewQuestionLists[InterviewQuestionListType.ALL]
                .Except(InterviewQuestionLists[InterviewQuestionListType.CURRENT])
                .Except(InterviewQuestionLists[InterviewQuestionListType.PREVIOUS])
                .Except(InterviewQuestionLists[InterviewQuestionListType.DISCARDED])
                .Random()
                .Reset(question.Title);

        InterviewQuestionLists[InterviewQuestionListType.CURRENT] = currentList;
    }

    public void FilterQuestions(string? search)
    {
        InterviewQuestionLists[InterviewQuestionListType.FILTERED] = 
            string.IsNullOrWhiteSpace(search)
                ? InterviewQuestionLists[InterviewQuestionListType.CURRENT]
                : InterviewQuestionLists[InterviewQuestionListType.CURRENT].Where(x => x.Contains(search));
    }

    public void UpdateQuestion(InterviewQuestion original, InterviewQuestion updated, InterviewQuestionListType listType = InterviewQuestionListType.CURRENT)
    {
        if (original is null)
        {
            throw new ArgumentNullException(nameof(original));
        }

        if (updated is null)
        {
            throw new ArgumentNullException(nameof(updated));
        }

        updated = new InterviewQuestion
        {
            Title = updated.Title is null ? original.Title : updated.Title,
            Category = updated.Category is null ? original.Category : updated.Category,
            Content = updated.Content is null ? original.Content : updated.Content,
            Difficulty = updated.Difficulty is null ? original.Difficulty : updated.Difficulty,
            IsPinned = updated.IsPinned is null ? original.IsPinned : updated.IsPinned,
            Note = updated.Note is null ? original.Note : updated.Note,
            Rating = updated.Rating == 0 ? original.Rating : updated.Rating
        };

        var list = InterviewQuestionLists[listType].ToList();

        list[list.IndexOf(original)] = updated;

        InterviewQuestionLists[listType] = list.AsEnumerable();
    }
}
