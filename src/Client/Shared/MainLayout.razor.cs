
using InterviewApp.Client.Constants;
using MudBlazor;

namespace InterviewApp.Client.Shared;

public partial class MainLayout
{
    MudTheme? currentTheme;

    protected override void OnInitialized()
    {
        currentTheme = ThemeConstants.LIGHT_THEME;
    }

    protected void SwitchTheme()
    {
        if (currentTheme == ThemeConstants.LIGHT_THEME)
        {
            currentTheme = ThemeConstants.DARK_THEME;
        }
        else
        {
            currentTheme = ThemeConstants.LIGHT_THEME;
        }
    }
}
