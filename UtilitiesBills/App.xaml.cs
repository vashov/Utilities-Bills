using System;
using UtilitiesBills.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace UtilitiesBills
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
