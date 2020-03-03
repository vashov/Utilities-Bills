using System;

namespace UtilitiesBills.Models
{
    public class MeterReadingItem : ICloneable
    {
        public int Id { get; set; }
        public string Note { get; set; }
        public decimal HotWaterBulk { get; set; }
        public decimal ColdWaterBulk { get; set; }
        public decimal ElectricityBulk { get; set; }
        public DateTime DateOfReading { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
