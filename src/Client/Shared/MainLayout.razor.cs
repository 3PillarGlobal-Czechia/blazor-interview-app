
using InterviewApp.Client.Constants;
using InterviewApp.Client.Services.Interface;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace InterviewApp.Client.Shared;

public partial class MainLayout
{
    private EventCallback onSwitchTheme => EventCallback.Factory.Create(this, SwitchTheme);

    private MudTheme? currentTheme;

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
