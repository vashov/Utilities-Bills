using Autofac;
using UtilitiesBills.Models;
using UtilitiesBills.Services;
using UtilitiesBills.Services.Bill;
using UtilitiesBills.Views;
using Xamarin.Forms;

namespace UtilitiesBills
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            
            //DependencyService.Register<MockBillRepository>();
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
