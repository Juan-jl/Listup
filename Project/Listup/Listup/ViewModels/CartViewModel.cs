using Listup.Helpers;
using Listup.Interfaces;
using Listup.Models;
using Listup.Repositories;
using Listup.Views;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Listup.ViewModels
{
    public class CartViewModel : INotifyPropertyChanged
    {
        private Cart _cart;
        private readonly INavigationService _navigationService;
        private readonly CartRepository _cartRepository = new CartRepository();
        private readonly CartItemRepository _cartItemRepository = new CartItemRepository();
        private FormatCurrency _formatCurrency;
        private CalculateTotal _calculateTotal;

        private string _cartTitle;
        private bool _isInCartMode;
        private bool _cartHasItems;
        private bool _hasItemsOutsideCart;
        private int _cartItemsQuantity;
        private string _currencySymbol;
        private string _priceCartItemInsertion;
        private int? _quantityCartItemInsertion;
        private string _nameCartItemInsertion;
        private string _grandTotal;
        private int _itensOutsideCartCount;
        private double _priceReminderValue;
        private int _quantityReminderValue;
        public bool isCartModeInternalUpdate;//Used by the code-behind to skip when the toggle was triggered by the ViewModel

        public ICommand AddNewCartItemCommand { get; }
        public ICommand PriceListChangedCommand { get; }
        public ICommand DeleteCartItemCommand { get; }
        public ICommand QuantityListChangedCommand { get; }
        public ICommand OpenCartDetailsCommand { get; }
        public ICommand NavigateBackCommand { get; }
        public ICommand ReviewTutorialCommand { get; }

        public event Action<string, string> OnAlertRequested;

        private ObservableCollection<CartItem> _cartItems = new ObservableCollection<CartItem>();

        public event Func<string, string, Task> AddPriceReminder;

        public event Func<string, string, Task> AddQuantityReminder;

        public ObservableCollection<CartItem> CartItems
        {
            get => _cartItems;
            set { if (_cartItems != value) { _cartItems = value; OnPropertyChanged(nameof(CartItems)); } }
        }

        public string CartTitle
        {
            get => _cartTitle;
            set { if (_cartTitle != value) { _cartTitle = value; OnPropertyChanged(nameof(CartTitle)); } }
        }

        public bool IsInCartMode
        {
            get => _isInCartMode;
            set
            {
                if (_isInCartMode != value)
                {
                    _isInCartMode = value;
                    isCartModeInternalUpdate = true;
                    OnPropertyChanged(nameof(IsInCartMode));
                    isCartModeInternalUpdate = false;
                    UpdateAddModeAsync(true);
                }
            }
        }

        public bool CartHasItems
        {
            get => _cartHasItems;
            set { if (_cartHasItems != value) { _cartHasItems = value; OnPropertyChanged(nameof(CartHasItems)); } }
        }

        public bool HasItemsOutsideCart
        {
            get => _hasItemsOutsideCart;
            set { if (_hasItemsOutsideCart != value) { _hasItemsOutsideCart = value; OnPropertyChanged(nameof(HasItemsOutsideCart)); } }
        }

        public int CartItemsQuantity
        {
            get => _cartItemsQuantity;
            set { if (_cartItemsQuantity != value) { _cartItemsQuantity = value; OnPropertyChanged(nameof(CartItemsQuantity)); } }
        }

        public string CurrencySymbol
        {
            get => _currencySymbol;
            set { if (_currencySymbol != value) { _currencySymbol = value; OnPropertyChanged(nameof(CurrencySymbol)); } }
        }

        public string PriceCartItemInsertion
        {
            get => _priceCartItemInsertion;
            set { if (_priceCartItemInsertion != value) { _priceCartItemInsertion = value; OnPropertyChanged(nameof(PriceCartItemInsertion)); } }
        }

        public int? QuantityCartItemInsertion
        {
            get => _quantityCartItemInsertion;
            set { if (_quantityCartItemInsertion != value) { _quantityCartItemInsertion = value; OnPropertyChanged(nameof(QuantityCartItemInsertion)); } }
        }

        public string NameCartItemInsertion
        {
            get => _nameCartItemInsertion;
            set { if (_nameCartItemInsertion != value) { _nameCartItemInsertion = value; OnPropertyChanged(nameof(NameCartItemInsertion)); } }
        }

        public string GrandTotal
        {
            get => _grandTotal;
            set { if (_grandTotal != value) { _grandTotal = value; OnPropertyChanged(nameof(GrandTotal)); } }
        }

        public int ItensOutsideCartCount
        {
            get => _itensOutsideCartCount;
            set { if (_itensOutsideCartCount != value) { _itensOutsideCartCount = value; OnPropertyChanged(nameof(ItensOutsideCartCount)); } }
        }

        public double PriceReminderValue
        {
            get => _priceReminderValue;
            set { if (_priceReminderValue != value) { _priceReminderValue = value; OnPropertyChanged(nameof(PriceReminderValue)); } }
        }

        public int QuantityReminderValue
        {
            get => _quantityReminderValue;
            set { if (_quantityReminderValue != value) { _quantityReminderValue = value; OnPropertyChanged(nameof(QuantityReminderValue)); } }
        }

        public CartViewModel(INavigationService navigationService, Cart currentCart)
        {
            _navigationService = navigationService;
            _cart = currentCart;
            _formatCurrency = new FormatCurrency(_cart);
            _calculateTotal = new CalculateTotal(_cart);

            AddNewCartItemCommand = new Command(async () => await AddNewCartItemAsync());
            PriceListChangedCommand = new Command<CartItem>(async (cartItem) => await UpdatePriceAsync(cartItem));
            DeleteCartItemCommand = new Command<CartItem>(async (cartItem) => await DeleteCartItemAsync(cartItem));
            QuantityListChangedCommand = new Command<CartItem>(async (cartItem) => await QuantityListChangedAsync(cartItem));
            OpenCartDetailsCommand = new Command(async () => await OpenCartDetails());
            NavigateBackCommand = new Command(async () => await NavigateBackAsync());
            ReviewTutorialCommand = new Command(async () => await ReviewTutorialAsync());

            MessagingCenter.Send<object>(this, "ClosePrevious");
            LoadElementsAsync();
            SubscribeMessages();
        }

        private async Task LoadElementsAsync()
        {
            CartTitle = _cart.Title;
            CurrencySymbol = _formatCurrency.GetCurrencySymbol();
            GrandTotal = $"{CurrencySymbol}{_formatCurrency.GetPriceFormatted(await _calculateTotal.GetGrandTotalAsync())}";
            ItensOutsideCartCount = await _cartRepository.GetItemsOutsideCartCountAsync(_cart);
            IsInCartMode = (_cart.AddMode == "cart");
            await LoadAllCartItemsAsync();
            await UpdateAddModeAsync(false);
        }

        public async Task UpdateCartTitleAsync(string newTitle)//Called by code-behind
        {
            if (!string.IsNullOrWhiteSpace(newTitle))
            {
                _cart.Title = newTitle;
                CartTitle = newTitle;
                await UpdateDataBaseCartAsync();
            }
        }

        private async Task LoadAllCartItemsAsync()
        {
            var cartItems = await _cartItemRepository.GetByCartIdAsync(_cart.IdCart);
            CartItems.Clear();

            foreach (var cartItem in cartItems)
            {
                cartItem.DisplayPrice = _formatCurrency.GetPriceFormatted(cartItem.Price);
                cartItem.DisplayTotalPrice = $"Total: {CurrencySymbol} {_formatCurrency.GetPriceFormatted(_calculateTotal.GetCartItemTotal(cartItem.Price, (double)cartItem.Quantity))}";
                CartItems.Add(cartItem);

                if (IsInCartMode)
                    cartItem.BgColorCartItemConteiner = cartItem.IsInCart ? (Xamarin.Forms.Color)Application.Current.Resources["MainContainerColor"] : (Xamarin.Forms.Color)Application.Current.Resources["SecondaryContainerColor"];
                else
                    cartItem.BgColorCartItemConteiner = (Xamarin.Forms.Color)Application.Current.Resources["SecondaryContainerColor"];
            }

            CartHasItems = CartItems.Count > 0;
            HasItemsOutsideCart = CartItems.Any(item => item.IsInCart == false);
            CartItemsQuantity = CartItems.Count;
        }

        private async Task AddNewCartItemAsync()
        {
            if (string.IsNullOrWhiteSpace(NameCartItemInsertion) || IsInCartMode && PriceCartItemInsertion == null)
            {
                OnAlertRequested?.Invoke("Alerta", "Preencha todos os campos");
                return;
            }

            if (QuantityCartItemInsertion == null)
                QuantityCartItemInsertion = 0;

            if (string.IsNullOrEmpty(PriceCartItemInsertion))
                PriceCartItemInsertion = _formatCurrency.ParsePrice("0.00").ToString();

            var newCartItem = new CartItem
            {
                IdCartFK = _cart.IdCart,
                Name = NameCartItemInsertion,
                Price = _formatCurrency.ParsePrice(PriceCartItemInsertion),
                Quantity = QuantityCartItemInsertion,
                IsInCart = IsInCartMode,
                DisplayPrice = PriceCartItemInsertion
            };

            newCartItem.BgColorCartItemConteiner = newCartItem.IsInCart ? (Xamarin.Forms.Color)Application.Current.Resources["MainContainerColor"] : (Xamarin.Forms.Color)Application.Current.Resources["SecondaryContainerColor"];

            PriceCartItemInsertion = null;
            NameCartItemInsertion = null;
            QuantityCartItemInsertion = 0;

            await _cartItemRepository.InsertAsync(newCartItem);

            newCartItem.DisplayTotalPrice = $"Total: {CurrencySymbol} {_formatCurrency.GetPriceFormatted(_calculateTotal.GetCartItemTotal(newCartItem.Price, (double)newCartItem.Quantity))}";
            GrandTotal = $"{CurrencySymbol}{_formatCurrency.GetPriceFormatted(await _calculateTotal.GetGrandTotalAsync())}";

            CartItems.Insert(0, newCartItem); //Adds the new cart item to the top of the list.
            ItensOutsideCartCount = await _cartRepository.GetItemsOutsideCartCountAsync(_cart);

            CartHasItems = (CartItems.Count > 0);
            HasItemsOutsideCart = CartItems.Any(item => item.IsInCart == false);

            CartItemsQuantity = CartItems.Count;
        }

        private async Task UpdateAddModeAsync(bool updateAddModeInDataBase)
        {
            foreach (var cartItem in _cartItems)
            {
                if (IsInCartMode)
                    cartItem.BgColorCartItemConteiner = cartItem.IsInCart ? (Xamarin.Forms.Color)Application.Current.Resources["MainContainerColor"] : (Xamarin.Forms.Color)Application.Current.Resources["SecondaryContainerColor"];
                else
                    cartItem.BgColorCartItemConteiner = (Xamarin.Forms.Color)Application.Current.Resources["SecondaryContainerColor"];
            }

            if (updateAddModeInDataBase)
                await UpdateDataBaseCartAsync();

            //The code that calls ChangedCartModeAlertPage is in the code-behind.
        }

        public async Task OpenChangedCartModeAlertPageAsync()//Called by Code-behind
        {
            await _navigationService.ShowModalAsync(new Listup.Views.ChangedCartModeAlertPage(IsInCartMode));
        }

        private async Task UpdateDataBaseCartAsync()
        {
            _cart.EditedDate = DateTime.Now;
            _cart.AddMode = "list";
            if (IsInCartMode)
                _cart.AddMode = "cart";

            await _cartRepository.UpdateAsync(_cart);
        }

        private async Task UpdateDataBaseCartItemAsync(CartItem item)
        {
            await _cartItemRepository.UpdateAsync(item);
            await UpdateDataBaseCartAsync();
        }

        private async Task UpdatePriceAsync(CartItem cartItem)
        {
            cartItem.Price = _formatCurrency.ParsePrice(cartItem.DisplayPrice);
            if (cartItem.Price < 0)
                cartItem.Price = 0.00;

            await UpdateDataBaseCartItemAsync(cartItem);

            cartItem.DisplayTotalPrice = $"Total: {CurrencySymbol} {_formatCurrency.GetPriceFormatted(_calculateTotal.GetCartItemTotal(cartItem.Price, (double)cartItem.Quantity))}";
            GrandTotal = $"{CurrencySymbol}{_formatCurrency.GetPriceFormatted(await _calculateTotal.GetGrandTotalAsync())}";
        }

        private async Task QuantityListChangedAsync(CartItem cartItem)
        {
            await UpdateDataBaseCartItemAsync(cartItem);
            cartItem.DisplayTotalPrice = $"Total: {CurrencySymbol} {_formatCurrency.GetPriceFormatted(_calculateTotal.GetCartItemTotal(cartItem.Price, (double)cartItem.Quantity))}";
            GrandTotal = $"{CurrencySymbol}{_formatCurrency.GetPriceFormatted(await _calculateTotal.GetGrandTotalAsync())}";
        }

        public async Task IsInCartListChanged(CartItem cartItem) // Called by Code-behind
        {
            if (cartItem.IsInCart)
            {
                if (cartItem.Price <= 0)
                {
                    await AddPriceReminder.Invoke("Adicione o preço", $"O item ({cartItem.Name}) não tem preço salvo. Você deve inseri-lo abaixo antes de adicioná-lo ao carrinho:");
                    cartItem.Price = PriceReminderValue;
                }
                if (cartItem.Quantity <= 0 || cartItem.Quantity == null)
                {
                    await AddQuantityReminder.Invoke("Adicione a quantidade", $"O item ({cartItem.Name}) não tem quantidade salva. Você deve inseri-la abaixo antes de adicioná-la ao carrinho:");
                    cartItem.Quantity = QuantityReminderValue;
                }
            }

            await UpdateDataBaseCartItemAsync(cartItem);
            cartItem.BgColorCartItemConteiner = cartItem.IsInCart ? (Xamarin.Forms.Color)Application.Current.Resources["MainContainerColor"] : (Xamarin.Forms.Color)Application.Current.Resources["SecondaryContainerColor"];
            cartItem.DisplayTotalPrice = $"Total: {CurrencySymbol} {_formatCurrency.GetPriceFormatted(_calculateTotal.GetCartItemTotal(cartItem.Price, (double)cartItem.Quantity))}";
            cartItem.DisplayPrice = _formatCurrency.GetPriceFormatted(cartItem.Price);
            OnPropertyChanged(nameof(cartItem.DisplayPrice));
            GrandTotal = $"{CurrencySymbol}{_formatCurrency.GetPriceFormatted(await _calculateTotal.GetGrandTotalAsync())}";
            ItensOutsideCartCount = await _cartRepository.GetItemsOutsideCartCountAsync(_cart);
            HasItemsOutsideCart = CartItems.Any(item => !item.IsInCart);
        }

        private async Task DeleteCartItemAsync(CartItem cartItem)
        {
            await _cartItemRepository.DeleteAsync(cartItem);
            CartItems.Remove(cartItem);

            CartItemsQuantity = CartItems.Count;
            CartHasItems = CartItems.Count > 0;
            HasItemsOutsideCart = CartItems.Any(item => item.IsInCart == false);

            GrandTotal = $"{CurrencySymbol}{_formatCurrency.GetPriceFormatted(await _calculateTotal.GetGrandTotalAsync())}";

            ItensOutsideCartCount = await _cartRepository.GetItemsOutsideCartCountAsync(_cart);
            await UpdateDataBaseCartAsync();
        }

        private async Task OpenCartDetails()
        {
            await _navigationService.ShowModalAsync(new Listup.Views.CartDetailsPage(_cart));
        }

        private async Task NavigateBackAsync()
        {
            await _navigationService.NavigateBackAsync();
        }

        private void SubscribeMessages()
        {
            MessagingCenter.Subscribe<CartDetailsViewModel>(this, "CloseCartScreen", async sender =>
            {
                await NavigateBackAsync();
            });

            MessagingCenter.Subscribe<CartDetailsPage>(this, "UpdateCartPageElements", async sender =>
            {
                await LoadElementsAsync();
            });
        }

        private async Task ReviewTutorialAsync()
        {
            await _navigationService.ShowModalAsync(new WelcomePage());
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}