using UtilitiesBills.Services.Navigation;
using UtilitiesBills.ViewModels.Base;
using Xamarin.Forms;

namespace UtilitiesBills
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            InitNavigation();
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

        private void InitNavigation()
        {
            var navigationService = ViewModelLocator.Resolve<INavigationService>();
            navigationService.Initialize();
        }
    }
}
