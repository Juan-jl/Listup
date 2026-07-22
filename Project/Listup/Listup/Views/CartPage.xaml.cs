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
    public partial class CartPage : ContentPage
    {
        public CartPage(Cart cart, bool isNewCart)
        {
            this.Resources.MergedDictionaries.Add(new CartStyles());
            InitializeComponent();
            var navigationService = new NavigationService();

            var viewModel = new CartViewModel(navigationService, cart);
            BindingContext = viewModel;

            if (isNewCart)
                SendSubscription();

            viewModel.OnAlertRequested += async (title, message) =>
            {
                await DisplayAlert(title, message, "OK");
            };

            viewModel.AddPriceReminder += async (title, message) =>
            {
                var input = await DisplayPromptAsync(title, message, "OK", null, $"{viewModel.CurrencySymbol} 00.00", -1, Keyboard.Numeric);
                if (double.TryParse(input, out double priceReminderValue))
                    viewModel.PriceReminderValue = priceReminderValue;
            };

            viewModel.AddQuantityReminder += async (title, message) =>
            {
                var input = await DisplayPromptAsync(title, message, "OK", null, "0", -1, Keyboard.Numeric);
                try
                {
                    viewModel.QuantityReminderValue = Convert.ToInt32(input);
                }
                catch (Exception e)
                {
                    viewModel.QuantityReminderValue = 0;
                }
            };
            InitialWarnings();
        }

        private async void InitialWarnings()
        {
            await DisplayAlert("Dica", "Arraste um item para excluí-lo.", "OK");
        }

        private async void OnCartTitleTapped(object sender, EventArgs e)
        {
            var vm = (CartViewModel)BindingContext;
            string newTitle = await DisplayPromptAsync(
                "Alterar nome da compra", null, "OK", "Cancelar", null, -1, null, vm.CartTitle);

            await vm.UpdateCartTitleAsync(newTitle);
        }

        private async void CheckBoxIsInCart_Tapped(object sender, EventArgs e) //This method was created in the code-behind to handle the GestureRecognizer and prevent the price reminder from appearing out of context.
        {
            if (sender is Grid grid && grid.Parent is Grid parentGrid && parentGrid.Children[0] is CheckBox cb && cb.BindingContext is CartItem item)
            {
                cb.IsChecked = !cb.IsChecked;

                var vm = (CartViewModel)BindingContext;
                await vm.IsInCartListChanged(item);
            }
        }

        public void SendSubscription()
        {
            MessagingCenter.Send<CartPage>(this, "ClosePrevious");
        }

        private async void CartModeSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            var vm = (CartViewModel)BindingContext;
            if (vm.isCartModeInternalUpdate) // Skip when the toggle was triggered by the ViewModel
                return;
            await vm.OpenChangedCartModeAlertPageAsync();
        }
    }
}