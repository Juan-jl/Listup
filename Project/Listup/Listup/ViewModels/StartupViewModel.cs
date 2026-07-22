using Listup.Interfaces;
using Listup.Models;
using Listup.Repositories;
using Listup.SQLiteData;
using Listup.Views;
using System.Threading.Tasks;

namespace Listup.ViewModels
{
    public class StartupViewModel
    {
        private readonly INavigationService _navigationService;

        public StartupViewModel(INavigationService navigationService)
        {
            this._navigationService = navigationService;
            _ = OpenNextPageAsync();
        }

        private async Task OpenNextPageAsync()
        {
            if (await IsUserOnboarding())
                await _navigationService.NavigateToAsync(new WelcomePage());
            else
                await _navigationService.NavigateToAsync(new HomePage());
        }

        private async Task<bool> IsUserOnboarding()
        {
            await DatabaseConnection.InitializeAsync();

            ConfigsRepository configsRepository = new ConfigsRepository();

            if (await configsRepository.CountConfigs() > 0)
            {
                Configs config = new Configs();
                config = await configsRepository.GetConfigRecordAsync();
                return config.IsUserOnboarding;
            }
            else
            {
                await configsRepository.InsertAsync();
                return true;
            }
        }
    }
}
