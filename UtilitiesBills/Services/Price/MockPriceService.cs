using System;

namespace UtilitiesBills.Services.Price
{
    public class MockPriceService : IPriceService
    {
        public decimal HotWaterPrice => 105;

        public decimal ColdWaterPrice => 18;

        public decimal WaterDisposalPrice => 13.8M;

        public decimal ElectricityPrice => 2.6M;
    }
}
