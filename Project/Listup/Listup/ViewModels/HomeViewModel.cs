using Listup.Helpers;
using Listup.Interfaces;
using Listup.Models;
using Listup.Repositories;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Listup.ViewModels
{
    public class HomeViewModel : INotifyPropertyChanged
    {
        private readonly INavigationService _navigationService;
        private CartRepository _cartRepository = new CartRepository();

        public event Func<string, string, string, string, Task<bool>> OnConfirmationRequested;

        public ICommand OpenCartScreenCommand { get; }
        public ICommand AddNewCartCommand { get; }
        public ICommand DeleteCartCommand { get; }
        public ICommand DetailsCartCommand { get; }
        public ICommand OpenSettingsPageCommand { get; }
        public ICommand OrderCartsByCommand { get; }

        private bool _hasCarts;
        private bool _isLoading;

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                if (_isLoading != value)
                {
                    _isLoading = value;
                    OnPropertyChanged(nameof(IsLoading));
                }
            }
        }

        public bool HasCarts
        {
            get => _hasCarts;
            set
            {
                if (_hasCarts != value)
                {
                    _hasCarts = value;
                    OnPropertyChanged(nameof(HasCarts));
                }
            }
        }

        public ObservableCollection<Cart> Carts { get; } = new ObservableCollection<Cart>();

        public HomeViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            OpenCartScreenCommand = new Command<Cart>(async (cart) => await OpenCartScreenAsync(cart));
            AddNewCartCommand = new Command(async () => await AddNewCartAsync());
            DeleteCartCommand = new Command<Cart>(async (cart) => await DeleteCart(cart));
            DetailsCartCommand = new Command<Cart>(async (cart) => await DetailsCart(cart));
            OpenSettingsPageCommand = new Command(async () => await OpenSettingsPageAsync());
            OrderCartsByCommand = new Command<CartOrderBy>(async (orderBy) => await OrderCartsByAsync(orderBy));
            HasCarts = false;
            _ = LoadCartsAsync();
        }

        public async Task OrderCartsByAsync(CartOrderBy orderBy) //Called by Code-Behind
        {
            var cartsOrdenados = await _cartRepository.GetAllAsync(orderBy);
            this.Carts.Clear();
            foreach (var cart in cartsOrdenados)
                this.Carts.Add(cart);
        }

        public async Task LoadCartsAsync() //Called by Code-Behind
        {
            IsLoading = true;
            HasCarts = false;

            var carts = await _cartRepository.GetAllAsync();
            this.Carts.Clear();

            foreach (var cart in carts)
            {
                Carts.Add(cart);
            }

            HasCarts = ExistsCarts();
            IsLoading = false;
        }

        private async Task OpenCartScreenAsync(Cart cart)
        {
            IsLoading = true;
            await Task.Yield(); // ensures the UI updates and displays the loading spinner

            var cartPage = new Listup.Views.CartPage(cart, false);
            await _navigationService.ShowModalAsync(cartPage);

            IsLoading = false;
        }

        private async Task AddNewCartAsync()
        {
            await _navigationService.ShowModalAsync(new Listup.Views.AddNewCartPage());
        }

        private async Task DeleteCart(Cart cart)
        {
            if (await ConfirmActionAsync("Excluir compra?", "Deseja mesmo deletar permanentemente essa compra?\nEssa ação não poderá ser desfeita!"))
            {
                await _cartRepository.DeleteAsync(cart);
                this.Carts.Remove(cart);
                HasCarts = ExistsCarts();
            }
        }

        private async Task DetailsCart(Cart cart)
        {
            await _navigationService.ShowModalAsync(new Listup.Views.CartDetailsPage(cart));
        }

        private async Task OpenSettingsPageAsync()
        {
            await _navigationService.ShowModalAsync(new Listup.Views.SettingsPage());
        }

        private bool ExistsCarts()
        {
            return Carts.Count > 0;
        }

        private async Task<bool> ConfirmActionAsync(string title, string message)
        {
            if (OnConfirmationRequested != null)
                return await OnConfirmationRequested.Invoke(title, message, "Ok", "Cancelar");
            return false;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
