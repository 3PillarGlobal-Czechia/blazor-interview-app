
using InterviewApp.Client.Enums;
using InterviewApp.Client.Extensions;
using InterviewApp.Client.Services.Interface;
using InterviewApp.Shared.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Net.Http.Json;

namespace InterviewApp.Client.Services;

public class InterviewService : IInterviewService
{
    private ISnackbar? _snackbar { get; set; }

    public IDictionary<QuestionListType, IEnumerable<InterviewQuestion>> QuestionLists { get; set; }

    public IList<string?> SelectedCategories { get; set; }

    private readonly HttpClient _http;

    public InterviewService(HttpClient http, ISnackbar snaccbar)
    {
        _http = http;
        _snackbar = snaccbar;

        QuestionLists = new Dictionary<QuestionListType, IEnumerable<InterviewQuestion>>();
        SelectedCategories = new List<string?>();
    }

    public bool IsInitialized()
    {
        return QuestionLists is not null &&
               QuestionLists.ContainsKey(QuestionListType.All) &&
               QuestionLists.ContainsKey(QuestionListType.Current);
    }

    public async Task Initialize()
    {
        if (!QuestionLists.ContainsKey(QuestionListType.All))
        {
            var dataset = await _http.GetFromJsonAsync<List<InterviewQuestion>>("Interview");
            if (dataset is not null)
            {
                QuestionLists.Add(QuestionListType.All, dataset);
            }
        }
    }

    public IList<string?> GetCategories()
    {
        if (QuestionLists is null || !QuestionLists.ContainsKey(QuestionListType.All))
        {
            throw new InvalidOperationException($"{nameof(QuestionLists)} is not initialized");
        }

        return QuestionLists[QuestionListType.All].Select(x => x.Category).Distinct().ToList();
    }

    public void PrepareInterviewQuestions(IList<string?> categories)
    {
        if (!QuestionLists.ContainsKey(QuestionListType.All))
        {
            throw new InvalidOperationException($"{nameof(QuestionLists)} cannot be null for {nameof(QuestionListType.All)}");
        }

        var current = new List<InterviewQuestion>();
        var previous = new List<InterviewQuestion>();
        var discarded = new List<InterviewQuestion>();

        if (QuestionLists.ContainsKey(QuestionListType.Current))
            current = QuestionLists[QuestionListType.Current].ToList();

        if (QuestionLists.ContainsKey(QuestionListType.Previous))
            previous = QuestionLists[QuestionListType.Previous].ToList();

        if (QuestionLists.ContainsKey(QuestionListType.Discarded))
            discarded = QuestionLists[QuestionListType.Discarded].ToList();

        SelectedCategories = categories;

        current = QuestionLists[QuestionListType.All].Where(q => SelectedCategories.Contains(q.Category)).Except(current).Except(previous).Except(discarded).Random(10).ToList();
        for (int i = 0; i < current.Count(); i++)
        {
            current[i].Reset(i);
        }

        QuestionLists.Remove(QuestionListType.Current);
        QuestionLists.Remove(QuestionListType.Previous);
        QuestionLists.Remove(QuestionListType.Filtered);
        QuestionLists.Remove(QuestionListType.Discarded);

        QuestionLists.Add(QuestionListType.Current, current.AsEnumerable());
        QuestionLists.Add(QuestionListType.Previous, previous.AsEnumerable());
        QuestionLists.Add(QuestionListType.Filtered, current.AsEnumerable());
        QuestionLists.Add(QuestionListType.Discarded, discarded.AsEnumerable());
    }

    public void ResetQuestions(IList<string?> categories)
    {
        PrepareInterviewQuestions(categories);
    }

    public void ResetQuestion(InterviewQuestion? question)
    {
        if (!QuestionLists.ContainsKey(QuestionListType.Current))
        {
            throw new InvalidOperationException($"{nameof(QuestionLists)} cannot be null for {nameof(QuestionListType.Current)}");
        }

        if (question is null)
        {
            throw new ArgumentNullException(nameof(question));
        }

        if (string.IsNullOrWhiteSpace(question.Title))
        {
            throw new InvalidOperationException($"{nameof(question.Title)} cannot be null or empty.");
        }

        var currentList = QuestionLists[QuestionListType.Current].ToList();
        var resetList = 
            QuestionLists[QuestionListType.All]
                .Where(q => SelectedCategories.Contains(q.Category))
                .Except(QuestionLists[QuestionListType.Current])
                .Except(QuestionLists[QuestionListType.Previous])
                .Except(QuestionLists[QuestionListType.Discarded]);

        if (resetList.Any())
        {
            currentList[currentList.IndexOf(question)] = resetList.Random().Reset(question.Title);

            var discardedList = QuestionLists[QuestionListType.Discarded].ToList();
            discardedList.Add(question);

            QuestionLists[QuestionListType.Current] = currentList;
            QuestionLists[QuestionListType.Discarded] = discardedList;
        }
        else
        {
            if (_snackbar is null)
            {
                throw new InvalidOperationException($"{nameof(_snackbar)} is not initialized.");
            }

            _snackbar.Add("No more available questions.", Severity.Error);
        }
    }

    public void FilterQuestions(string? search)
    {
        QuestionLists[QuestionListType.Filtered] = 
            string.IsNullOrWhiteSpace(search)
                ? QuestionLists[QuestionListType.Current]
                : QuestionLists[QuestionListType.Current].Where(q => q.Contains(search));
    }

    public void UpdateQuestion(InterviewQuestion original, InterviewQuestion updated, QuestionListType listType = QuestionListType.Current)
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

        var list = QuestionLists[listType].ToList();

        list[list.IndexOf(original)] = updated;

        QuestionLists[listType] = list.AsEnumerable();
    }
}
