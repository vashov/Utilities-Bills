namespace UtilitiesBills.Services.Price
{
    public interface IPriceService
    {
        decimal HotWaterPrice { get; }
        decimal ColdWaterPrice { get; }
        decimal WaterDisposalPrice { get; }
        decimal ElectricityPrice { get; }
    }
}
