using UtilitiesBills.Helpers;
using UtilitiesBills.ViewModels.Base;
using Xamarin.Essentials;

namespace UtilitiesBills.Services.Settings
{
    public class SettingsService : BaseNotifier, ISettingsService
    {
        private const string DATABASENAME = "utilities.db3";

        private bool _isDarkTheme;
        private decimal _initHotWaterBulk;
        private decimal _initColdWaterBulk;
        private decimal _initElectricityBulk;
        private decimal _defaultHotWaterPrice;
        private decimal _defaultColdWaterPrice;
        private decimal _defaultElectricityPrice;
        private decimal _defaultWaterDisposalPrice;
        private string _emailForSendBackup;
        private string _emailForSendLogs;

        public bool IsDarkTheme 
        { 
            get => _isDarkTheme;
            set
            {
                if (SetProperty(ref _isDarkTheme, value))
                {
                    AddOrUpdateValue(SettingsKeys.IsDarkTheme, value);
                }
            } 
        }

        public decimal InitHotWaterBulk 
        { 
            get => _initHotWaterBulk;
            set
            {
                if (SetProperty(ref _initHotWaterBulk, value))
                {
                    AddOrUpdateValue(SettingsKeys.InitHotWaterBulk, value);
                }
            }
        }

        public decimal InitColdWaterBulk 
        { 
            get => _initColdWaterBulk;
            set
            {
                if (SetProperty(ref _initColdWaterBulk, value))
                {
                    AddOrUpdateValue(SettingsKeys.InitColdWaterBulk, value);
                }
            }
        }

        public decimal InitElectricityBulk 
        { 
            get => _initElectricityBulk;
            set
            {
                if (SetProperty(ref _initElectricityBulk, value))
                {
                    AddOrUpdateValue(SettingsKeys.InitElectricityBulk, value);
                }
            }
        }

        public decimal DefaultHotWaterPrice 
        { 
            get => _defaultHotWaterPrice;
            set
            {
                if (SetProperty(ref _defaultHotWaterPrice, value))
                {
                    AddOrUpdateValue(SettingsKeys.DefaultHotWaterPrice, value);
                }
            }
        }

        public decimal DefaultColdWaterPrice 
        { 
            get => _defaultColdWaterPrice;
            set
            {
                if (SetProperty(ref _defaultColdWaterPrice, value))
                {
                    AddOrUpdateValue(SettingsKeys.DefaultColdWaterPrice, value);
                }
            }
        }

        public decimal DefaultElectricityPrice 
        { 
            get => _defaultElectricityPrice;
            set
            {
                if (SetProperty(ref _defaultElectricityPrice, value))
                {
                    AddOrUpdateValue(SettingsKeys.DefaultElectricityPrice, value);
                }
            }
        }

        public decimal DefaultWaterDisposalPrice
        {
            get => _defaultWaterDisposalPrice;
            set
            {
                if (SetProperty(ref _defaultWaterDisposalPrice, value))
                {
                    AddOrUpdateValue(SettingsKeys.DefaultWaterDisposalPrice, value);
                }
            }
        }

        public string EmailForSendBackup
        {
            get => _emailForSendBackup;
            set
            {
                if (SetProperty(ref _emailForSendBackup, value))
                {
                    AddOrUpdateValue(SettingsKeys.EmailForSendBackup, value);
                }
            }
        }

        public string EmailForSendLogs
        {
            get => _emailForSendLogs;
            set
            {
                if (SetProperty(ref _emailForSendLogs, value))
                {
                    AddOrUpdateValue(SettingsKeys.EmailForSendLogs, value);
                }
            }
        }

        public string DatabasePath { get; private set; }

        public SettingsService()
        {
            _isDarkTheme = GetValue(SettingsKeys.IsDarkTheme, defaultValue: false);

            _initHotWaterBulk = GetValue(SettingsKeys.InitHotWaterBulk, defaultValue: 0);
            _initColdWaterBulk = GetValue(SettingsKeys.InitColdWaterBulk, defaultValue: 0);
            _initElectricityBulk = GetValue(SettingsKeys.InitElectricityBulk, defaultValue: 0);

            _defaultHotWaterPrice = GetValue(SettingsKeys.DefaultHotWaterPrice, defaultValue: 0);
            _defaultColdWaterPrice = GetValue(SettingsKeys.DefaultColdWaterPrice, defaultValue: 0);
            _defaultElectricityPrice = GetValue(SettingsKeys.DefaultElectricityPrice, defaultValue: 0);
            _defaultWaterDisposalPrice = GetValue(SettingsKeys.DefaultWaterDisposalPrice, defaultValue: 0);

            _emailForSendBackup = GetValue(SettingsKeys.EmailForSendBackup, defaultValue: string.Empty);
            _emailForSendLogs = GetValue(SettingsKeys.EmailForSendLogs, defaultValue: string.Empty);

            DatabasePath = FileHelper.GetLocalPath(DATABASENAME);
        }

        private void AddOrUpdateValue(string key, bool value)
        {
            Preferences.Set(key, value);
        }

        private void AddOrUpdateValue(string key, decimal value)
        {
            Preferences.Set(key, (double)value);
        }

        private void AddOrUpdateValue(string key, string value)
        {
            Preferences.Set(key, value);
        }

        private bool GetValue(string key, bool defaultValue)
        {
            return Preferences.Get(key, defaultValue);
        }

        private decimal GetValue(string key, double defaultValue)
        {
            return (decimal) Preferences.Get(key, defaultValue);
        }

        private string GetValue(string key, string defaultValue)
        {
            return Preferences.Get(key, defaultValue);
        }
    }
}
