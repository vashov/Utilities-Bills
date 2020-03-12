using UtilitiesBills.Services.Settings;

namespace UtilitiesBills.Services.Price
{
    public class PriceService : IPriceService
    {
        private ISettingsService SettingsService { get; set; }

        public PriceService(ISettingsService settingsService)
        {
            SettingsService = settingsService;
        }

        public decimal HotWaterPrice => SettingsService.DefaultHotWaterPrice;

        public decimal ColdWaterPrice => SettingsService.DefaultColdWaterPrice;

        public decimal WaterDisposalPrice => SettingsService.DefaultWaterDisposalPrice;

        public decimal ElectricityPrice => SettingsService.DefaultElectricityPrice;
    }
}
