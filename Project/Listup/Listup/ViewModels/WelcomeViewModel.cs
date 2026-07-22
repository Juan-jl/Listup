using Listup.Interfaces;
using Listup.Models;
using Listup.Repositories;
using Listup.Views;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Listup.ViewModels
{
    public class WelcomeViewModel
    {
        private Configs _configs;
        private readonly INavigationService _navigationService;
        private readonly ConfigsRepository _configsRepository = new ConfigsRepository();

        public ICommand OpenNextPageCommand { get; }

        public WelcomeViewModel(INavigationService navigationService)
        {
            this._navigationService = navigationService;
            OpenNextPageCommand = new Command(async () => await OpenNextPageAsync());
        }

        private async Task OpenNextPageAsync()
        {
            _configs = await _configsRepository.GetConfigRecordAsync();
            if (_configs.IsUserOnboarding)
                await _navigationService.NavigateToAsync(new CurrencySelectionPage(true));
            else
                await _navigationService.NavigateBackAsync();
        }
    }
}
