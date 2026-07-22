using Listup.Helpers;
using Listup.Services;
using Listup.Styles;
using Listup.ViewModels;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Listup.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WelcomePage : ContentPage
    {
        private readonly WelcomeViewModel viewModel;
        int currentFrame = 0;
        public WelcomePage()
        {
            this.Resources.MergedDictionaries.Add(new WelcomeStyles());
            InitializeComponent();
            var navigationService = new NavigationService();
            viewModel = new WelcomeViewModel(navigationService);
            BindingContext = viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await ShowFirstFrame();
            SetDarkAndLightImages();
        }

        private async Task ShowFirstFrame()
        {
            Frame1.TranslationX = 1000;
            Frame2.TranslationX = 1000;
            Frame3.TranslationX = 1000;

            Frame2.IsVisible = false;
            Frame3.IsVisible = false;
            BtnMoveFrameBackwards.Opacity = 0;
            await Frame1.TranslateTo(0, 0, 2000, Easing.CubicOut);
            currentFrame = 1;
        }

        private async void MoveFrameForward_Clicked(object sender, EventArgs e)
        {
            if (currentFrame == 1)
            {
                currentFrame = 2;
                _ = BtnMoveFrameBackwards.FadeTo(1, 500);
                await Frame1.TranslateTo(-1000, 0, 0500, Easing.CubicOut);
                Frame1.IsVisible = false;
                Frame2.IsVisible = true;
                await Frame2.TranslateTo(0, 0, 0500, Easing.CubicOut);
            }
            else if (currentFrame == 2)
            {
                currentFrame = 3;
                BtnMoveFrameForward.Text = "Concluir";
                await Frame2.TranslateTo(-1000, 0, 0500, Easing.CubicOut);
                Frame2.IsVisible = false;
                Frame3.IsVisible = true;
                await Frame3.TranslateTo(0, 0, 0500, Easing.CubicOut);
            }
            else if (currentFrame == 3)
            {
                _ = BtnMoveFrameForward.FadeTo(0, 500);
                _ = BtnMoveFrameBackwards.FadeTo(0, 500);
                await Frame3.FadeTo(0, 500);
                viewModel.OpenNextPageCommand.Execute(null);
            }
        }

        private async void MoveFrameBackwards_Clicked(object sender, EventArgs e)
        {
            if (currentFrame == 2)
            {
                currentFrame = 1;
                _ = BtnMoveFrameBackwards.FadeTo(0, 500);

                await Frame2.TranslateTo(1000, 0, 0500, Easing.CubicOut);
                Frame2.IsVisible = false;
                Frame1.IsVisible = true;
                await Frame1.TranslateTo(0, 0, 0500, Easing.CubicOut);
            }
            else if (currentFrame == 3)
            {
                currentFrame = 2;
                BtnMoveFrameForward.Text = "Próximo";
                await Frame3.TranslateTo(1000, 0, 0500, Easing.CubicOut);
                Frame3.IsVisible = false;
                Frame2.IsVisible = true;
                await Frame2.TranslateTo(0, 0, 0500, Easing.CubicOut);
            }
        }
        private void SetDarkAndLightImages()
        {
            if (ThemeManager.IsDarkTheme())
            {
                WelcomeImageFrame2.Source = "WelcomeImageFrame2Dark.png";
                WelcomeImageFrame3.Source = "WelcomeImageFrame3Dark.png";
            }
            else
            {
                WelcomeImageFrame2.Source = "WelcomeImageFrame2Light.png";
                WelcomeImageFrame3.Source = "WelcomeImageFrame3Light.png";
            }
        }
    }
}