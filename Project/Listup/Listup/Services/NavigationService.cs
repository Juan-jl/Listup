using Listup.Interfaces;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Listup.Services
{
    public class NavigationService : INavigationService
    {
        public async Task ShowModalAsync(Page page)
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(page);
        }

        public async Task NavigateBackAsync()
        {
            await Application.Current.MainPage.Navigation.PopModalAsync();
        }
        public async Task NavigateToAsync(Page page)
        {
            Application.Current.MainPage = page;
            await Task.CompletedTask;
        }
    }
}