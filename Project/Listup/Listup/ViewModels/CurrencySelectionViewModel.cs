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
    public class CurrencySelectionViewModel : INotifyPropertyChanged
    {
        private Configs _configs;
        private readonly INavigationService _navigationService;
        private readonly ConfigsRepository _configsRepository = new ConfigsRepository();
        private readonly CartRepository cartRepository = new CartRepository();

        public event Func<string, string, string, string, Task<CurrencyChangeOption>> OnConfirmChangeCurrencyRequested;

        private ObservableCollection<CurrencyInfo> _currenciesList;

        public ICommand ChangeCurrencyCommand { get; }
        public ICommand NavigateBackCommand { get; }

        private bool _isUserOnboarding;

        public ObservableCollection<CurrencyInfo> CurrenciesList
        {
            get => _currenciesList;
            set
            {
                if (_currenciesList != value)
                {
                    _currenciesList = value;
                    OnPropertyChanged(nameof(CurrenciesList));
                }
            }
        }

        public bool IsUserOnboarding
        {
            get => _isUserOnboarding;
            set { if (_isUserOnboarding != value) { _isUserOnboarding = value; OnPropertyChanged(nameof(IsUserOnboarding)); } }
        }

        public CurrencySelectionViewModel(INavigationService navigationService, bool isUserOnboarding)
        {
            this._navigationService = navigationService;

            ChangeCurrencyCommand = new Command<CurrencyInfo>(async currency => await ChangeCurrencyAsync(currency));
            NavigateBackCommand = new Command(async () => await NavigateBackAsync());

            LoadConfigsAsync();
            IsUserOnboarding = isUserOnboarding;

            LoadElements();
        }

        private async Task LoadConfigsAsync()
        {
            _configs = await _configsRepository.GetConfigRecordAsync();
        }

        private void LoadElements()
        {
            CurrenciesList = new ObservableCollection<CurrencyInfo>(CurrencyInfo.All);
        }

        private async Task ChangeCurrencyAsync(CurrencyInfo currency)
        {
            _configs.DefaultCurrencyCode = currency.Code;
            await _configsRepository.UpdateAsync(_configs);
            _configs.IsUserOnboarding = false;

            if (!IsUserOnboarding)
            {
                var option = await ConfirmChangeCurrencyAsync();

                if (option != CurrencyChangeOption.Cancel)
                {
                    if (option == CurrencyChangeOption.Existing)
                    {
                        await cartRepository.SetAllCurrenciesToSameValue(currency.Code);
                    }
                }
                else
                    return;
            }
            await _navigationService.NavigateToAsync(new Views.HomePage());
            await _configsRepository.UpdateAsync(_configs);
        }

        private async Task<CurrencyChangeOption> ConfirmChangeCurrencyAsync()
        {
            if (OnConfirmChangeCurrencyRequested != null)
                return await OnConfirmChangeCurrencyRequested.Invoke(
                    "Em quais compras deseja alterar a moeda?",
                    "Apenas em compras novas",
                    "Nas já existentes e nas novas",
                    "Cancelar"
                );

            return CurrencyChangeOption.Cancel;
        }

        private async Task NavigateBackAsync()
        {
            await _navigationService.NavigateBackAsync();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName) =>
             PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
