
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
    public string? SearchValue
    {
        get => _searchValue;
        set
        {
            _searchValue = value;

            FilterList();
        }
    }

    [Inject]
    private IInterviewService? _interviewService { get; set; }

    [Inject]
    private IDialogService? _dialogService { get; set; }

    [Inject]
    private ISnackbar? _snackbar { get; set; }

    private string? _searchValue;

    protected override async Task OnInitializedAsync()
    {
        if (_interviewService is null)
        {
            throw new ArgumentNullException(nameof(_interviewService));
        }

        await _interviewService.Initialize();
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

        if (_interviewService.InterviewQuestionLists[InterviewQuestionListType.CURRENT].Contains(question))
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

        if (!_interviewService.InterviewQuestionLists[InterviewQuestionListType.CURRENT].Any(x => x.Rating > 0))
        {
            _snackbar.Add(InterviewConstants.ReportSnackbarNoRatingText, Severity.Error);

            return;
        }

        if (_dialogService is null)
        {
            throw new ArgumentNullException(nameof(_dialogService));
        }

        var parameters = new DialogParameters();
        parameters.Add(InterviewConstants.ReportDialogQuestionsParameter, _interviewService.InterviewQuestionLists[InterviewQuestionListType.CURRENT]);

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

        var dialogParams = new DialogParameters();
        DialogResult? result;

        if (question is null)
        {
            dialogParams.Add(InterviewConstants.ResetAllDialogContentParameter, InterviewConstants.ResetAllDialogContent);

            result = await _dialogService.Show<ResetDialog>(InterviewConstants.ResetAllDialogTitle, dialogParams).Result;
        }
        else
        {
            dialogParams.Add(InterviewConstants.ResetDialogContentParameter, string.Format(InterviewConstants.ResetDialogContent, question.Title));

            result = await _dialogService.Show<ResetDialog>(string.Format(InterviewConstants.ResetDialogTitle, question.Title), dialogParams).Result;
        }

        if (result?.Data is not null && (bool)result.Data)
        {
            _interviewService.ResetQuestions(question);

            FilterList();
        }
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
}
