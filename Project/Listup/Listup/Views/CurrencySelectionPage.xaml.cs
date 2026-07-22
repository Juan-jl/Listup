using Listup.Models;
using Listup.Services;
using Listup.Styles;
using Listup.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Listup.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CurrencySelectionPage : ContentPage
    {
        public CurrencySelectionPage(bool isFirstAppAccess)
        {
            Resources.MergedDictionaries.Add(new CurrencySelectionStyles());
            InitializeComponent();

            var navigationService = new NavigationService();
            var viewModel = new CurrencySelectionViewModel(navigationService, isFirstAppAccess);
            BindingContext = viewModel;

            viewModel.OnConfirmChangeCurrencyRequested += async (title, option1, option2, cancel) =>
            {
                var result = await DisplayActionSheet(title, cancel, null, option1, option2);

                if (result == option1)
                    return CurrencyChangeOption.NewOnly;
                if (result == option2)
                    return CurrencyChangeOption.Existing;
                return CurrencyChangeOption.Cancel;
            };
        }
    }
}