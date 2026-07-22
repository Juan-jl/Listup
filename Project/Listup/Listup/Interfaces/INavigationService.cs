using System.Threading.Tasks;
using Xamarin.Forms;

namespace Listup.Interfaces
{
    public interface INavigationService
    {
        Task ShowModalAsync(Page page);
        Task NavigateBackAsync();
        Task NavigateToAsync(Page page);
    }
}