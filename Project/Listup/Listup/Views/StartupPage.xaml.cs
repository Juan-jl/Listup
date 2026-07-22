using Listup.Services;
using Listup.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Listup.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StartupPage : ContentPage
    {
        public StartupPage()
        {
            InitializeComponent();
            var navigationService = new NavigationService();
            var viewModel = new StartupViewModel(navigationService);
            BindingContext = viewModel;
        }
    }
}