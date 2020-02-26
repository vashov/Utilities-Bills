using Autofac;
using System;
using UtilitiesBills.Models;
using UtilitiesBills.Services;
using UtilitiesBills.Services.Bill;
using UtilitiesBills.Services.Navigation;
using UtilitiesBills.ViewModels.Base;
using UtilitiesBills.Views;
using Xamarin.Forms;

namespace UtilitiesBills
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            //MainPage = new MainMasterDetailView();
            InitNavigation();
        }

        protected override void OnStart()
        {
        }

        private void InitNavigation()
        {
            var navigationService = ViewModelLocator.Resolve<INavigationService>();
            navigationService.Initialize();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
