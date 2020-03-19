using UtilitiesBills.Services.Navigation;
using UtilitiesBills.ViewModels.Base;
using Xamarin.Forms;

namespace UtilitiesBills
{
    public partial class App : Application
    {
        private static INavigationService _navigationService = null;

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
            if (_navigationService == null)
            {
                _navigationService = ViewModelLocator.Resolve<INavigationService>();
            }
            _navigationService.Initialize();
        }
    }
}
