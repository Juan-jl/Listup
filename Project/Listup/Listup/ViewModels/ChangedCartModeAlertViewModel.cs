using Listup.Interfaces;
using System.Threading.Tasks;

namespace Listup.ViewModels
{
    public class ChangedCartModeAlertViewModel
    {
        private readonly INavigationService _navigationService;

        private string _addModeText;
        private string _addModeIconText;

        public string AddModeText
        {
            get => _addModeText;
            set
            {
                if (_addModeText != value)
                {
                    _addModeText = value;
                }
            }
        }

        public string AddModeIconText
        {
            get => _addModeIconText;
            set
            {
                if (_addModeIconText != value)
                {
                    _addModeIconText = value;
                }
            }
        }

        public ChangedCartModeAlertViewModel(INavigationService navigationService, bool isInCartMode)
        {
            _navigationService = navigationService;
            if (isInCartMode)
            {
                AddModeText = "MODO CARRINHO";
                AddModeIconText = "\uf07a";
            }
            else
            {
                AddModeText = "MODO LISTA";
                AddModeIconText = "\uf303;";
            }

            _= ClosePageAsync();
        }

        private async Task ClosePageAsync()
        {
            await Task.Delay(2000);
            await _navigationService.NavigateBackAsync();
        }
    }
}
