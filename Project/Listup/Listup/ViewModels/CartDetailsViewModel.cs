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
    public class CartDetailsViewModel : INotifyPropertyChanged
    {
        private Cart _cart;
        private readonly INavigationService _navigationService;
        private readonly CartRepository _cartRepository = new CartRepository();

        private string _cartTitle;
        private string _cartDescription;
        private DateTime _editedDate;
        private DateTime _creationDate;
        private int _selectedPickerIndex;

        public event Func<string, string, string, string, Task<bool>> OnConfirmationRequested;

        public ObservableCollection<string> PickerItems { get; set; } = new ObservableCollection<string>();

        public ICommand NavigateBackCommand { get; }
        public ICommand DeleteCartCommand { get; }

        public string CartTitle
        {
            get => _cartTitle;
            set { if (_cartTitle != value) { _cartTitle = value; OnPropertyChanged(nameof(CartTitle)); } }
        }
        public string CartDescription
        {
            get => _cartDescription;
            set { if (_cartDescription != value) { _cartDescription = value; OnPropertyChanged(nameof(CartDescription)); } }
        }

        public DateTime EditedDate
        {
            get => _editedDate;
            set { if (_editedDate != value) { _editedDate = value; OnPropertyChanged(nameof(EditedDate)); } }
        }

        public DateTime CreationDate
        {
            get => _creationDate;
            set { if (_creationDate != value) { _creationDate = value; OnPropertyChanged(nameof(CreationDate)); } }
        }

        public int SelectedPickerIndex
        {
            get => _selectedPickerIndex;
            set
            {
                if (_selectedPickerIndex != value)
                {
                    _selectedPickerIndex = value; OnPropertyChanged(nameof(SelectedPickerIndex));
                    _ = UpdateCurrencyAsync();
                }
            }
        }

        public CartDetailsViewModel(INavigationService navigationService, Cart cart)
        {
            _navigationService = navigationService;

            _cart = cart;
            NavigateBackCommand = new Command(async () => await NavigateBackAsync());
            DeleteCartCommand = new Command<Cart>(async (_cart) => await DeleteCartAsync());
            LoadElements();
        }

        private void LoadElements()
        {
            CartTitle = _cart.Title;
            CartDescription = _cart.Description;
            CreationDate = _cart.CreationDate;
            EditedDate = _cart.EditedDate;

            int i = 0;
            foreach (var currency in CurrencyInfo.All)
            {
                PickerItems.Add($"{currency.Name} ({currency.Symbol})");
                if (currency.Code == _cart.Currency)
                    SelectedPickerIndex = i;
                i++;
            }
            OnPropertyChanged(nameof(PickerItems));
            OnPropertyChanged(nameof(SelectedPickerIndex));

        }

        private async Task UpdateCurrencyAsync()
        {
            _cart.Currency = CurrencyInfo.All[SelectedPickerIndex].Code;
            await UpdateDataBaseCartAsync();
        }

        public async Task UpdateDataBaseCartAsync() //Called also by Code-Behind
        {
            _cart.Title = CartTitle;
            _cart.Description = CartDescription;
            _cart.EditedDate = DateTime.Now;
            EditedDate = _cart.EditedDate;
            await _cartRepository.UpdateAsync(_cart);
        }

        private async Task NavigateBackAsync()
        {
            await UpdateDataBaseCartAsync();
            await _navigationService.NavigateBackAsync();
        }

        private async Task DeleteCartAsync()
        {
            if (await ConfirmActionAsync("Excluir compra?", "Deseja mesmo deletar permanentemente essa compra?\nEssa ação não poderá ser desfeita!"))
            {
                await _cartRepository.DeleteAsync(_cart);
                MessagingCenter.Send<CartDetailsViewModel>(this, "CloseCartScreen");
                await NavigateBackAsync();
            }
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
