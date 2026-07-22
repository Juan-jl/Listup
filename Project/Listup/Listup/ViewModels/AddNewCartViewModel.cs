using Listup.Interfaces;
using Listup.Models;
using Listup.Repositories;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Listup.ViewModels
{
    public class AddNewCartViewModel : INotifyPropertyChanged
    {
        private readonly INavigationService _navigationService;

        private bool _isLoading;

        public ICommand AddNewCartCommand { get; }
        public ICommand CloseScreenCommand { get; }

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

        public AddNewCartViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            AddNewCartCommand = new Command<object>(async (param) => await AddNewCartAsync(param));
            CloseScreenCommand = new Command(async () => await CloseScreenAsync());

            IsLoading = false;
        }

        private async Task AddNewCartAsync(object param)
        {
            IsLoading = true;

            CartRepository cartRepository = new CartRepository();
            ConfigsRepository configsRepository = new ConfigsRepository();
            Configs config = await configsRepository.GetConfigRecordAsync();

            var newCart = new Cart
            {
                Title = "Compra sem nome",
                Description = "",
                CreationDate = DateTime.Now,
                EditedDate = DateTime.Now,
                AddMode = param?.ToString() ?? "cart",
                Currency = config.DefaultCurrencyCode
            };

            await cartRepository.InsertAsync(newCart);

            newCart.Title = $"COMPRA {newCart.IdCart}";
            await cartRepository.UpdateAsync(newCart);

            bool isNewCart = true;
            await _navigationService.ShowModalAsync(new Listup.Views.CartPage(newCart, isNewCart));
        }

        private async Task CloseScreenAsync()
        {
            await _navigationService.NavigateBackAsync();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}