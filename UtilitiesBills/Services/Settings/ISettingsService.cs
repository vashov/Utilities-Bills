namespace UtilitiesBills.Services.Settings
{
    public interface ISettingsService
    {
        bool IsDarkTheme { get; set; }

        decimal InitHotWaterBulk { get; set; }
        decimal InitColdWaterBulk { get; set; }
        decimal InitElectricityBulk { get; set; }

        decimal DefaultHotWaterPrice { get; set; }
        decimal DefaultColdWaterPrice { get; set; }
        decimal DefaultElectricityPrice { get; set; }
        decimal DefaultWaterDisposalPrice { get; set; }

        string DatabasePath { get; }
        string EmailForSendBackup { get; set; }
        string EmailForSendLogs { get; set; }
    }
}
