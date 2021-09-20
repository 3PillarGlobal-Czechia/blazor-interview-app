
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace InterviewApp.Client.Dialogs;

public partial class SetupDialog
{
    [CascadingParameter]
    MudDialogInstance? MudDialog { get; set; }

    [Parameter] 
    public IList<string>? Categories { get; set; }

    public IList<string>? SelectedCategories { get; set; } = new List<string>();

    void Submit()
        => MudDialog?.Close(DialogResult.Ok(SelectedCategories));

    void ToggleCategory(string category)
    {
        if (Categories is null)
        {
            throw new InvalidOperationException($"{nameof(Categories)} is not initialized.");
        }

        if (SelectedCategories is null)
        {
            throw new InvalidOperationException($"{nameof(SelectedCategories)} is not initialized.");
        }

        if (Categories.Contains(category))
        {
            Categories.Remove(category);
            SelectedCategories.Add(category);
        }
        else if (SelectedCategories.Contains(category))
        {
            SelectedCategories.Remove(category);
            Categories.Add(category);
        }
    }
}
