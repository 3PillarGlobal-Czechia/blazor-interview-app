
using InterviewApp.Client.Dialogs;
using InterviewApp.Client.Extensions;
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

    [Inject]
    private IClipboardService? _clipboardService { get; set; }

    [Inject]
    private ISnackbar? _snackbar { get; set; }

    private Random _random = new Random();

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
        if (_interviewService is null)
        {
            throw new ArgumentNullException(nameof(_interviewService));
        }

        _all = await _interviewService.GetInterviewQuestions();

        Randomize();
    }

    protected void Randomize()
    {
        if (_all is null)
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
            var index = _random.Next(0, _all.Count);

            if (_currIndexList.Contains(index) || (_prevIndexList is not null && _prevIndexList.Contains(index)))
            {
                i--;
            }
            else
            {
                var question = _all[index];
                question.Title = $"Question {i + 1}";
                question.IsPinned = false;
                question.Rating = 0;
                question.Note = null;

                _currIndexList.Add(index);
                _current.Add(question);
            }
        }

        _currentFiltered = _current;
    }

    protected void RandomizeSingle(InterviewQuestion oldQuestion)
    {
        if (_all is null)
        {
            throw new ArgumentNullException(nameof(_all));
        }

        if (_pinned is null)
        {
            throw new ArgumentNullException(nameof(_pinned));
        }

        if (_discarded is null)
        {
            throw new ArgumentNullException(nameof(_discarded));
        }

        if (_current is null)
        {
            throw new ArgumentNullException(nameof(_current));
        }

        if (_currIndexList is null)
        {
            throw new ArgumentNullException(nameof(_currIndexList));
        }

        _pinned = _pinned.Except(new[] { oldQuestion }).ToList();

        var newQuestion = _all.Except(_current).Except(_discarded).Except(new[] { oldQuestion }).Random(_random);
        newQuestion.Title = oldQuestion.Title;

        _current[_current.IndexOf(oldQuestion)] = newQuestion;

        FilterList();

        _discarded.Add(oldQuestion);
    }

    protected void Reset()
    {
        _prevIndexList = _currIndexList;

        Randomize();
    }

    protected void FilterList(string? search = null)
    {
        if (_current is null)
        {
            throw new ArgumentNullException(nameof(_current));
        }

        if (_pinned is null)
        {
            throw new ArgumentNullException(nameof(_pinned));
        }

        search = search ?? SearchValue;

        if (string.IsNullOrEmpty(search))
        {
            _currentFiltered = _current;
            _pinnedFiltered = _pinned;
        }
        else
        {
            _currentFiltered = _current.Where(x => (x.Content is not null && x.Content.ToLower().Contains(search.ToLower())) ||
                                                   (x.Category is not null && x.Category.ToLower().Contains(search.ToLower())) ||
                                                   (x.Note is not null && x.Note.ToLower().Contains(search.ToLower()))).ToList();

            _pinnedFiltered = _pinned.Where(x => (x.Content is not null && x.Content.ToLower().Contains(search.ToLower())) ||
                                                 (x.Category is not null && x.Category.ToLower().Contains(search.ToLower())) ||
                                                 (x.Note is not null && x.Note.ToLower().Contains(search.ToLower()))).ToList();
        }
    }

    protected void TogglePin(InterviewQuestion question)
    {
        if (_current is null)
        {
            throw new ArgumentNullException(nameof(_current));
        }

        if (_pinned is null)
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
        if (_dialogService is null)
        {
            throw new ArgumentNullException(nameof(_dialogService));
        }

        var dialogParams = new DialogParameters();
        dialogParams.Add("Content", $"{question.Title} will be replaced with different question.");

        var result = await _dialogService.Show<ResetDialog>($"Reset {question.Title}", dialogParams).Result;

        if (result?.Data is not null && (bool)result.Data)
        {
            RandomizeSingle(question);
        }
    }

    protected void OpenReportDialog()
    {
        if (_current is null)
        {
            throw new ArgumentNullException(nameof(_current));
        }
        if (_snackbar is null)
        {
            throw new ArgumentNullException(nameof(_snackbar));
        }

        if (!_current.Any(x => x.Rating > 0))
        {
            _snackbar.Add("No rated questions!", Severity.Error);

            return;
        }

        if (_dialogService is null)
        {
            throw new ArgumentNullException(nameof(_dialogService));
        }

        var parameters = new DialogParameters();
        parameters.Add("Questions", _current);

        var options = new DialogOptions()
        {
            MaxWidth = MaxWidth.Small,
            FullWidth = true
        };

        _dialogService.Show<ReportDialog>("Interview Report", parameters, options);
    }

    protected async Task OpenResetAllDialog()
    {
        if (_dialogService is null)
        {
            throw new ArgumentNullException(nameof(_dialogService));
        }

        var dialogParams = new DialogParameters();
        dialogParams.Add("Content", $"All questions will be replaced with different ones.");

        var result = await _dialogService.Show<ResetDialog>("Reset All Questions", dialogParams).Result;

        if (result?.Data is not null && (bool)result.Data)
        {
            Randomize();
        }
    }

    protected async void OpenNoteDialog(InterviewQuestion question)
    {
        if (_dialogService is null)
        {
            throw new ArgumentNullException(nameof(_dialogService));
        }

        var parameters = new DialogParameters();
        parameters.Add("Note", question.Note);
        parameters.Add("Question", question.Content);

        var options = new DialogOptions()
        {
            MaxWidth = MaxWidth.Small,
            FullWidth = true,
            DisableBackdropClick = true
        };

        var result = await _dialogService.Show<NoteDialog>($"{question.Title} notes", parameters, options).Result;

        if (result?.Data is not null)
        {
            if (_current is null)
            {
                throw new ArgumentNullException(nameof(_current));
            }

            if (_pinned is null)
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