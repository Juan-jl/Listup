using Listup.Interfaces;
using Listup.Styles;
using Xamarin.Forms;

namespace Listup.Services
{
    public static class ThemeManager
    {
        public static void Init()
        {
            ApplyTheme();
            Application.Current.RequestedThemeChanged += (s, e) => ApplyTheme(); //Monitors theme changes
        }

        public static void ApplyTheme()
        {
            Application.Current.Resources.MergedDictionaries.Clear();

            bool blackIcons = true;

            if (Application.Current.RequestedTheme == OSAppTheme.Dark)
            {
                Application.Current.Resources.MergedDictionaries.Add(new ColorsDark());
                blackIcons = false;
            }
            else
                Application.Current.Resources.MergedDictionaries.Add(new ColorsLight());

            Application.Current.Resources.MergedDictionaries.Add(new GlobalStyles());

            //The conversions to hexadecimal are done to apply the color dynamically:
            var StatusBarColor = (Color)Application.Current.Resources["StatusBarColor"];
            string StatusBarColorHex = $"#{(int)(StatusBarColor.R * 255):X2}{(int)(StatusBarColor.G * 255):X2}{(int)(StatusBarColor.B * 255):X2}";
            DependencyService.Get<IAndroidColorManager>()?.SetStatusBarColor(StatusBarColorHex, blackIcons);

            var NavigationBarColor = (Color)Application.Current.Resources["NavigationBarColor"];
            string NavigationBarColorHex = $"#{(int)(NavigationBarColor.R * 255):X2}{(int)(NavigationBarColor.G * 255):X2}{(int)(NavigationBarColor.B * 255):X2}";
            DependencyService.Get<IAndroidColorManager>()?.SetNavigationBarColor(NavigationBarColorHex, blackIcons);
        }

        public static bool IsDarkTheme()
        {
            return Application.Current.RequestedTheme == OSAppTheme.Dark;
        }
    }
}