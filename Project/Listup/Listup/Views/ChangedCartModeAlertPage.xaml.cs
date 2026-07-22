using Listup.Services;
using Listup.Styles;
using Listup.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Listup.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChangedCartModeAlertPage : ContentPage
    {
        public ChangedCartModeAlertPage(bool isInCartMode)
        {
            this.Resources.MergedDictionaries.Add(new ChangedCartModeAlertStyles());
            InitializeComponent();
            var navigationService = new NavigationService();
            BindingContext = new ChangedCartModeAlertViewModel(navigationService, isInCartMode);
        }
    }
}