using Listup.Helpers;
using Listup.Interfaces;
using Listup.Views;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Listup.ViewModels
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        private readonly INavigationService _navigationService;

        private ContactBuilder _contactBuilder = new ContactBuilder();

        public ICommand ChangeCurrencyCommand { get; }
        public ICommand NavigateBackCommand { get; }
        public ICommand OpenLinkGitHubCommand { get; }
        public ICommand StillInDevelopmentCommand { get; }
        public ICommand ReviewTutorialCommand { get; }

        public event Action<string, string> OnAlertRequested;

        private string _contact;

        public string Contact
        {
            get => _contact;
            set { if (_contact != value) { _contact = value; OnPropertyChanged(nameof(Contact)); } }
        }

        public SettingsViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            ChangeCurrencyCommand = new Command(async () => await ChangeCurrencyAsync());
            NavigateBackCommand = new Command(async () => await NavigateBackAsync());
            OpenLinkGitHubCommand = new Command(async () => await OpenLinkGitHubAsync());
            StillInDevelopmentCommand = new Command(() => StillInDevelopment());
            ReviewTutorialCommand = new Command(async () => await ReviewTutorialAsync());

            Contact = _contactBuilder.Generator();
        }

        private async Task ChangeCurrencyAsync()
        {
            bool isFirstAppAccess = false;
            await _navigationService.ShowModalAsync(new CurrencySelectionPage(isFirstAppAccess));
        }

        private async Task NavigateBackAsync()
        {
            await _navigationService.NavigateBackAsync();
        }

        private void StillInDevelopment()
        {
            OnAlertRequested?.Invoke("Não disponível", "Essa função ainda está em desenvolvimento.");
        }

        private async Task OpenLinkGitHubAsync()
        {
            await Launcher.OpenAsync("https://github.com/Juan-jl");
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
