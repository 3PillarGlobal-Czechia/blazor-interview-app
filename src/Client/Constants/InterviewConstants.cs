
namespace InterviewApp.Client.Constants;

public static class InterviewConstants
{
    // General constants
    public static readonly int MaxQuestionCount = 10;

    // Report constants
    public static readonly string ReportDialogQuestionsParameter = "Questions";
    public static readonly string ReportDialogTitle = "Interview Report";
    public static readonly string ReportSnackbarNoRatingText = "No rated questions.";
    public static readonly string ReportSnackbarReportCopiedText = "Report text copied.";
    public static readonly string ReportDownloadFileName = "report.txt";

    // Reset constants
    public static readonly string ResetDialogContentParameter = "Content";
    public static readonly string ResetDialogTitle = "Reset {0}";
    public static readonly string ResetDialogContent = "{0} will be replaced with a different question.";
    public static readonly string ResetAllDialogContentParameter = "Content";
    public static readonly string ResetAllDialogTitle = "Reset All Questions";
    public static readonly string ResetAllDialogContent = "All questions will be replaced with different ones.";

    // Note constants
    public static readonly string NoteDialogNoteParameter = "Note";
    public static readonly string NoteDialogQuestionParameter = "Question";
    public static readonly string NoteDialogTitle = "{0} notes";
}
