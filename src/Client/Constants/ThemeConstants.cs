
using MudBlazor;

namespace InterviewApp.Client.Constants;

public static class ThemeConstants
{
    public static readonly MudTheme LIGHT_THEME = new MudTheme
    {
        Palette = new Palette
        {
            Primary = "#0B2F35",
            Background = "#F9F9F9"
        }
    };

    public static readonly MudTheme DARK_THEME = new MudTheme
    {
        Palette = new Palette
        {
            Primary = "#12505B",
            Black = "#27272f",
            Background = "#061C1F",
            BackgroundGrey = "#27272f",
            Surface = "#0B2F35",
            DrawerBackground = "#27272f",
            DrawerText = "rgba(255,255,255, 0.50)",
            DrawerIcon = "rgba(255,255,255, 0.50)",
            AppbarBackground = "#27272f",
            AppbarText = "rgba(255,255,255, 0.70)",
            TextPrimary = "rgba(255,255,255, 0.70)",
            TextSecondary = "rgba(255,255,255, 0.50)",
            ActionDefault = "#adadb1",
            ActionDisabled = "rgba(255,255,255, 0.26)",
            ActionDisabledBackground = "rgba(255,255,255, 0.12)",
            Divider = "rgba(255,255,255, 0.12)",
            DividerLight = "rgba(255,255,255, 0.06)",
            TableLines = "rgba(255,255,255, 0.12)",
            LinesDefault = "rgba(255,255,255, 0.12)",
            LinesInputs = "rgba(255,255,255, 0.3)",
            TextDisabled = "rgba(255,255,255, 0.2)",
            GrayDarker = "#061C1F"
        }
    };
}
