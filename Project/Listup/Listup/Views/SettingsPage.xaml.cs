using Listup.Services;
using Listup.Styles;
using Listup.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Listup.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            this.Resources.MergedDictionaries.Add(new SettingsStyles());
            InitializeComponent();

            var navigationService = new NavigationService();
            var viewModel = new SettingsViewModel(navigationService);
            BindingContext = viewModel;

            viewModel.OnAlertRequested += async (title, message) =>
            {
                await DisplayAlert(title, message, "OK");
            };
        }
    }
}