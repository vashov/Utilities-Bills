using UtilitiesBills.ViewModels.Base;
using Xamarin.Forms;

namespace UtilitiesBills.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private bool _isDarkTheme;

        public bool IsDarkTheme 
        { 
            get => _isDarkTheme; 
            set => SetProperty(ref _isDarkTheme, value, UpdateDarkThemeSettings); 
        }

        public Command ShowBackupInfoCommand { get; private set; }
        public Command ShowInitialCounterValuesCommand { get; private set; }
        public Command ShowPricesCommand { get; private set; }
        public Command ShowLogsReportCommand { get; private set; }

        public SettingsViewModel()
        {
            InitCommands();
        }

        public override void Initialize(object navigationData)
        {
            IsDarkTheme = SettingsService.IsDarkTheme;
        }

        private void InitCommands()
        {
            ShowBackupInfoCommand = new Command(ShowBackupInfo);
            ShowInitialCounterValuesCommand = new Command(ShowInitialCounterValues);
            ShowPricesCommand = new Command(ShowPrices);
            ShowLogsReportCommand = new Command(ShowLogsReport);
        }

        private void UpdateDarkThemeSettings()
        {
            if (SettingsService.IsDarkTheme != IsDarkTheme)
            {
                SettingsService.IsDarkTheme = IsDarkTheme;
            }
        }

        private void ShowInitialCounterValues()
        {
            NavigationService.NavigateTo<InitialCounterEditorViewModel>();
        }

        private void ShowBackupInfo()
        {
            NavigationService.NavigateTo<BackupInfoViewModel>();
        }

        private void ShowPrices()
        {
            NavigationService.NavigateTo<DefaultPricesEditorViewModel>();
        }

        private void ShowLogsReport()
        {
            NavigationService.NavigateTo<LogsReportViewModel>();
        }
    }
}
