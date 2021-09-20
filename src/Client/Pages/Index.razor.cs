
using InterviewApp.Client.Constants;
using InterviewApp.Client.Dialogs;
using InterviewApp.Client.Enums;
using InterviewApp.Client.Services.Interface;
using InterviewApp.Shared.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace InterviewApp.Client.Pages;

public partial class Index
{
    [CascadingParameter]
    public EventCallback OnSwitchTheme { get; set; }

    public string? SearchValue
    {
        get => _searchValue;
        set
        {
            _searchValue = value;

            FilterList();
        }
    }

    private string? _searchValue;

    [Inject]
    private IInterviewService? _interviewService { get; set; }

    [Inject]
    private IDialogService? _dialogService { get; set; }

    [Inject]
    private ISnackbar? _snackbar { get; set; }

    protected override async Task OnInitializedAsync()
    {
        if (_interviewService is null)
        {
            throw new ArgumentNullException(nameof(_interviewService));
        }

        if (_dialogService is null)
        {
            throw new ArgumentNullException(nameof(_dialogService));
        }

        await _interviewService.Initialize();

        var parameters = new DialogParameters();
        parameters.Add(InterviewConstants.SetupDialogCategoriesParameter, _interviewService.GetCategories());

        var options = new DialogOptions
        {
            MaxWidth = MaxWidth.Small,
            FullWidth = true,
            CloseButton = false,
            DisableBackdropClick = true
        };

        var result = await _dialogService.Show<SetupDialog>(InterviewConstants.SetupDialogTitle, parameters, options).Result;
        var categories = result.Data as IList<string?>;

        if (categories is not null && categories.Count > 0)
        {
            _interviewService.PrepareInterviewQuestions(categories);
        }
    }

    protected void FilterList(string? search = null)
    {
        if (_interviewService is null)
        {
            throw new ArgumentNullException(nameof(_interviewService));
        }

        _interviewService.FilterQuestions(string.IsNullOrWhiteSpace(search) ? SearchValue : search);

        StateHasChanged();
    }

    protected void TogglePin(InterviewQuestion question)
    {
        if (_interviewService is null || !_interviewService.IsInitialized())
        {
            throw new ArgumentNullException(nameof(_interviewService));
        }

        if (_interviewService.QuestionLists[QuestionListType.Current].Contains(question))
        {
            _interviewService.UpdateQuestion(question, new InterviewQuestion { IsPinned = !question.IsPinned });

            FilterList();
        }
    }

    protected void OpenReportDialog()
    {
        if (_interviewService is null || !_interviewService.IsInitialized())
        {
            throw new ArgumentNullException(nameof(_interviewService));
        }

        if (_snackbar is null)
        {
            throw new ArgumentNullException(nameof(_snackbar));
        }

        if (!_interviewService.QuestionLists[QuestionListType.Current].Any(x => x.Rating > 0))
        {
            _snackbar.Add(InterviewConstants.ReportSnackbarNoRatingText, Severity.Error);

            return;
        }

        if (_dialogService is null)
        {
            throw new ArgumentNullException(nameof(_dialogService));
        }

        var parameters = new DialogParameters();
        parameters.Add(InterviewConstants.ReportDialogQuestionsParameter, _interviewService.QuestionLists[QuestionListType.Current]);

        var options = new DialogOptions
        {
            MaxWidth = MaxWidth.Small,
            FullWidth = true
        };

        _dialogService.Show<ReportDialog>(InterviewConstants.ReportDialogTitle, parameters, options);
    }

    protected async Task OpenResetDialog(InterviewQuestion? question = null)
    {
        if (_dialogService is null)
        {
            throw new ArgumentNullException(nameof(_dialogService));
        }

        if (_interviewService is null)
        {
            throw new ArgumentNullException(nameof(_interviewService));
        }

        var parameters = new DialogParameters();
        DialogResult? result;
        DialogOptions? options;

        if (question is not null)
        {
            parameters.Add(InterviewConstants.ResetDialogContentParameter, string.Format(InterviewConstants.ResetDialogContent, question.Title));
            result = await _dialogService.Show<ResetDialog>(string.Format(InterviewConstants.ResetDialogTitle, question.Title), parameters).Result;

            if (result?.Data is not null && (bool)result.Data)
            {
                _interviewService.ResetQuestion(question);
            }
        }
        else
        {
            parameters.Add(InterviewConstants.ResetAllDialogContentParameter, InterviewConstants.ResetAllDialogContent);
            result = await _dialogService.Show<ResetDialog>(InterviewConstants.ResetAllDialogTitle, parameters).Result;

            if (result?.Data is not null && (bool)result.Data)
            {
                parameters = new DialogParameters();
                parameters.Add(InterviewConstants.SetupDialogCategoriesParameter, _interviewService.GetCategories());

                options = new DialogOptions
                {
                    MaxWidth = MaxWidth.Small,
                    CloseButton = false,
                    FullWidth = true,
                };

                result = await _dialogService.Show<SetupDialog>(InterviewConstants.SetupDialogTitle, parameters, options).Result;
                var categories = result.Data as IList<string?>;

                if (categories is null)
                {
                    throw new InvalidDataException($"{nameof(categories)} is null, dialog result is corrupted.");
                }

                _interviewService.ResetQuestions(categories);
            }
        }

        FilterList();
    }

    protected async void OpenNoteDialog(InterviewQuestion question)
    {
        if (_interviewService is null)
        {
            throw new ArgumentNullException(nameof(_interviewService));
        }

        if (_dialogService is null)
        {
            throw new ArgumentNullException(nameof(_dialogService));
        }

        var parameters = new DialogParameters();
        parameters.Add(InterviewConstants.NoteDialogNoteParameter, question.Note);
        parameters.Add(InterviewConstants.NoteDialogQuestionParameter, question.Content);

        var options = new DialogOptions()
        {
            MaxWidth = MaxWidth.Small,
            FullWidth = true,
            DisableBackdropClick = true
        };

        var result = await _dialogService.Show<NoteDialog>(string.Format(InterviewConstants.NoteDialogTitle, question.Title), parameters, options).Result;

        if (result?.Data is not null)
        {
            if (!_interviewService.IsInitialized())
            {
                throw new ArgumentNullException(nameof(_interviewService));
            }

            _interviewService.UpdateQuestion(question, new InterviewQuestion { Note = result.Data.ToString() });

            FilterList();
        }
    }

    protected void RatingChanged()
    {
        StateHasChanged();
    }

    protected async Task SwitchTheme()
        => await OnSwitchTheme.InvokeAsync();

    protected IEnumerable<InterviewQuestion> Search(string value)
    {
        if (_interviewService is null)
        {
            throw new InvalidOperationException($"{nameof(_interviewService)} cannot be null.");
        }

        FilterList(value);

        return _interviewService.QuestionLists[QuestionListType.Filtered];
    }

    protected IEnumerable<InterviewQuestion> Search(InterviewQuestion value)
    {
        if (_interviewService is null)
        {
            throw new InvalidOperationException($"{nameof(_interviewService)} cannot be null.");
        }

        _interviewService.QuestionLists[QuestionListType.Filtered] =
            _interviewService.QuestionLists[QuestionListType.Current].Where(q => q.Equals(value));

        StateHasChanged();

        return _interviewService.QuestionLists[QuestionListType.Filtered];
    }
}
