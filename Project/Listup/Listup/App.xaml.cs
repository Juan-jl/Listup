using Listup.Services;
using Listup.Views;
using Xamarin.Forms;

[assembly: ExportFont("FontAwesome6Free-Solid-900.otf", Alias = "FontAwesomeSolid")]

namespace Listup
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            ThemeManager.Init(); // Apply the theme using the helper class
            MainPage = new StartupPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
