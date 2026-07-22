using Listup.Services;
using Listup.Styles;
using Listup.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Listup.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddNewCartPage : ContentPage
    {
        public AddNewCartPage()
        {
            this.Resources.MergedDictionaries.Add(new AddNewCartStyles());
            InitializeComponent();
            SubscribeMessages();
            var navigationService = new NavigationService();
            BindingContext = new AddNewCartViewModel(navigationService);
        }

        private void SubscribeMessages()
        {
            MessagingCenter.Subscribe<CartPage>(this, "ClosePrevious", async (sender) =>
            {
                if (Application.Current.MainPage.Navigation.ModalStack.Count > 0)
                    await Application.Current.MainPage.Navigation.PopModalAsync();
                MessagingCenter.Unsubscribe<CartPage>(this, "ClosePrevious");
            });
        }
    }
}