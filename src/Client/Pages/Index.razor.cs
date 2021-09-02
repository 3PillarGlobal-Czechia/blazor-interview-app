
using InterviewApp.Client.Dialogs;
using InterviewApp.Client.Services.Interface;
using InterviewApp.Shared.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace InterviewApp.Client.Pages;

public partial class Index
{
    public string? SearchValue
    {
        get => _searchValue;
        set
        {
            _searchValue = value;
            StateHasChanged();
            FilterList();
        }
    }

    [Inject]
    private IInterviewService? _interviewService { get; set; }

    [Inject]
    private IDialogService? _dialogService { get; set; }

    private Random random = new Random();

    private List<InterviewQuestion>? _all;
    private List<InterviewQuestion>? _current;
    private List<InterviewQuestion>? _currentFiltered;
    private List<InterviewQuestion>? _pinned;
    private List<InterviewQuestion>? _pinnedFiltered;
    private List<InterviewQuestion>? _discarded;
    private List<int>? _currIndexList;
    private List<int>? _prevIndexList;

    private string? _searchValue;

    protected override async Task OnInitializedAsync()
    {
        if (_interviewService == null)
        {
            throw new ArgumentNullException(nameof(_interviewService));
        }

        _all = await _interviewService.GetInterviewQuestions();

        Randomize();
    }

    protected void Randomize()
    {
        if (_all == null)
        {
            throw new ArgumentNullException(nameof(_all));
        }

        _current = new List<InterviewQuestion>();
        _currentFiltered = new List<InterviewQuestion>();
        _pinned = new List<InterviewQuestion>();
        _pinnedFiltered = new List<InterviewQuestion>();
        _discarded = new List<InterviewQuestion>();
        _currIndexList = new List<int>();

        for (int i = 0; i < 10; i++)
        {
            var index = random.Next(0, _all.Count);

            if (_currIndexList.Contains(index) || (_prevIndexList != null && _prevIndexList.Contains(index)))
            {
                i--;
            }
            else
            {
                var question = _all[index];
                question.Title = $"Question {i + 1}";
                question.IsPinned = false;
                question.Rating = 0;

                _currIndexList.Add(index);
                _current.Add(question);
            }
        }

        _currentFiltered = _current;
    }

    protected void RandomizeSingle(InterviewQuestion oldQuestion)
    {
        if (_all == null)
        {
            throw new ArgumentNullException(nameof(_all));
        }

        if (_pinned == null)
        {
            throw new ArgumentNullException(nameof(_pinned));
        }

        if (_discarded == null)
        {
            throw new ArgumentNullException(nameof(_discarded));
        }

        if (_current == null)
        {
            throw new ArgumentNullException(nameof(_current));
        }

        if (_currIndexList == null)
        {
            throw new ArgumentNullException(nameof(_currIndexList));
        }

        var found = false;

        while (!found)
        {
            var index = random.Next(0, _all.Count);

            if (!_currIndexList.Contains(index))
            {
                var newQuestion = _all[index];

                if (!_discarded.Contains(newQuestion))
                {
                    newQuestion.Title = oldQuestion.Title;
                    newQuestion.IsPinned = oldQuestion.IsPinned;

                    _current[_current.IndexOf(oldQuestion)] = newQuestion;

                    if (_pinned.Contains(oldQuestion))
                    {
                        _pinned[_pinned.IndexOf(oldQuestion)] = newQuestion;
                    }

                    FilterList();

                    found = true;
                }
            }
        }

        _discarded.Add(oldQuestion);
    }

    protected void Reset()
    {
        _prevIndexList = _currIndexList;

        Randomize();
    }

    protected void FilterList(string? searchVal = null)
    {
        if (_current == null)
        {
            throw new ArgumentNullException(nameof(_current));
        }

        if (_pinned == null)
        {
            throw new ArgumentNullException(nameof(_pinned));
        }

        var value = searchVal ?? _searchValue?.Trim();

        if (string.IsNullOrEmpty(value))
        {
            _currentFiltered = _current;
            _pinnedFiltered = _pinned;
        }
        else
        {
            _currentFiltered = _current.Where(x => (x.Content != null && x.Content.ToLower().Contains(value.ToLower())) ||
                                                   (x.Category != null && x.Category.ToLower().Contains(value.ToLower()))).ToList();

            _pinnedFiltered = _pinned.Where(x => (x.Content != null && x.Content.ToLower().Contains(value.ToLower())) ||
                                                 (x.Category != null && x.Category.ToLower().Contains(value.ToLower()))).ToList();
        }
    }

    protected void TogglePin(InterviewQuestion question)
    {
        if (_current == null)
        {
            throw new ArgumentNullException(nameof(_current));
        }

        if (_pinned == null)
        {
            throw new ArgumentNullException(nameof(_pinned));
        }

        if (_pinned.Contains(question))
        {
            _current[_current.IndexOf(question)].IsPinned = false;
            _pinned.Remove(question);
        }
        else
        {
            _current[_current.IndexOf(question)].IsPinned = true;
            _pinned.Add(question);
        }

        FilterList();
    }

    protected async Task OpenResetDialog(InterviewQuestion question)
    {
        if (_dialogService == null)
        {
            throw new ArgumentNullException(nameof(_dialogService));
        }

        var dialogParams = new DialogParameters();
        dialogParams.Add("Content", $"{question.Title} will be replaced with different question.");

        var result = await _dialogService.Show<ResetDialog>($"Reset {question.Title}", dialogParams).Result;

        if (result?.Data != null && (bool)result.Data)
        {
            RandomizeSingle(question);
        }
    }

    protected async Task OpenResetAllDialog()
    {
        if (_dialogService == null)
        {
            throw new ArgumentNullException(nameof(_dialogService));
        }

        var dialogParams = new DialogParameters();
        dialogParams.Add("Content", $"All questions will be replaced with different ones.");

        var result = await _dialogService.Show<ResetDialog>("Reset All Questions", dialogParams).Result;

        if (result?.Data != null && (bool)result.Data)
        {
            Randomize();
        }
    }

    protected async void OpenNoteDialog(InterviewQuestion question)
    {
        if (_dialogService == null)
        {
            throw new ArgumentNullException(nameof(_dialogService));
        }

        var dialogParams = new DialogParameters();
        dialogParams.Add("Note", question.Note);
        dialogParams.Add("Question", question.Content);

        var dialogOptions = new DialogOptions()
        {
            MaxWidth = MaxWidth.Small,
            FullWidth = true,
            DisableBackdropClick = true
        };

        var result = await _dialogService.Show<NoteDialog>($"{question.Title} notes", dialogParams, dialogOptions).Result;

        if (result?.Data != null)
        {
            if (_current == null)
            {
                throw new ArgumentNullException(nameof(_current));
            }

            if (_pinned == null)
            {
                throw new ArgumentNullException(nameof(_pinned));
            }

            _current[_current.IndexOf(question)].Note = (string)result.Data;

            var pinnedIndex = _pinned.IndexOf(question);
            if (pinnedIndex >= 0)
            {
                _pinned[pinnedIndex].Note = (string)result.Data;
            }

            FilterList();
            StateHasChanged();
        }
    }
}