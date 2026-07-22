using Listup.Models;
using Listup.Repositories;
using Listup.Services;
using Listup.Styles;
using Listup.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Listup.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            this.Resources.MergedDictionaries.Add(new HomeStyles());
            InitializeComponent();
            var navigationService = new NavigationService();
            var viewModel = new HomeViewModel(navigationService);
            BindingContext = viewModel;

            viewModel.OnConfirmationRequested += async (title, message, accept, cancel) =>
            {
                return await DisplayAlert(title, message, accept, cancel);
            };
        }

        private async void OnOrderByLabelTapped(object sender, EventArgs e)
        {
            var options = new Dictionary<string, CartOrderBy>
            {
                { "Título A-Z", CartOrderBy.TitleAsc },
                { "Título Z-A", CartOrderBy.TitleDesc },
                { "Data de edição", CartOrderBy.EditedDate },
                { "Data de criação", CartOrderBy.CreationDate }
            };

            string action = await DisplayActionSheet("Ordenar por", "Cancelar", null, options.Keys.ToArray());

            if (action == null || action == "Cancelar" || !options.TryGetValue(action, out var orderBy))
                return;

            if (BindingContext is HomeViewModel vm && vm.OrderCartsByCommand.CanExecute(orderBy))
                vm.OrderCartsByCommand.Execute(orderBy);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            (BindingContext as HomeViewModel)?.LoadCartsAsync();
        }
    }
}