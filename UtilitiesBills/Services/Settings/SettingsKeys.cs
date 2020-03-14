namespace UtilitiesBills.Services.Settings
{
    public static class SettingsKeys
    {
        public static string IsDarkTheme { get; } = "IsDarkTheme";

        public static string InitHotWaterBulk { get; } = "InitHotWaterBulk";
        public static string InitColdWaterBulk { get; } = "InitColdWaterBulk";
        public static string InitElectricityBulk { get; } = "InitElectricityBulk";

        public static string DefaultHotWaterPrice { get; } = "DefaultHotWaterPrice";
        public static string DefaultColdWaterPrice { get; } = "DefaultColdWaterPrice";
        public static string DefaultElectricityPrice { get; } = "DefaultElectricityPrice";
        public static string DefaultWaterDisposalPrice { get; } = "DefaultWaterDisposalPrice";

        public static string EmailForSendBackup { get; } = "EmailForSendBackup";
    }
}
