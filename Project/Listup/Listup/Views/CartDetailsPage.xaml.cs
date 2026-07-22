using Listup.Models;
using Listup.Services;
using Listup.Styles;
using Listup.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Listup.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CartDetailsPage : ContentPage
    {

        public CartDetailsPage(Cart cart)
        {
            this.Resources.MergedDictionaries.Add(new CartDetailsStyles());
            InitializeComponent();

            var navigationService = new NavigationService();
            var viewModel = new CartDetailsViewModel(navigationService, cart);

            viewModel.OnConfirmationRequested += async (title, message, accept, cancel) =>
            {
                return await DisplayAlert(title, message, accept, cancel);
            };

            BindingContext = viewModel;

            this.Disappearing += CartDetailsPage_Disappearing;
        }

        private async void UpdateDataAsync(object sender, EventArgs e)
        {
            var vm = BindingContext as CartDetailsViewModel;
            if (vm != null)
                await vm.UpdateDataBaseCartAsync();
        }

        private void CartDetailsPage_Disappearing(object sender, EventArgs e)
        {
            UpdateDataAsync(this, EventArgs.Empty);
            MessagingCenter.Send<CartDetailsPage>(this, "UpdateCartPageElements");
        }
    }
}