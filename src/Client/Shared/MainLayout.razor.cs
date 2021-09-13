
using InterviewApp.Client.Constants;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace InterviewApp.Client.Shared;

public partial class MainLayout
{
    EventCallback OnSwitchTheme => EventCallback.Factory.Create(this, SwitchTheme);

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
