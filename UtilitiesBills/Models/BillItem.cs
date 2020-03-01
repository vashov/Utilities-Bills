using System;

namespace UtilitiesBills.Models
{
    public class BillItem : ICloneable
    {
        public int Id { get; set; }
        public string Note { get; set; }
        public MeterReadingItem MeterReading { get; set; }
        public decimal WaterDisposalBulk { get; set; }
        public decimal HotWaterPrice { get; set; }
        public decimal ColdWaterPrice { get; set; }
        public decimal ElectricityPrice { get; set; }
        public decimal WaterDisposalPrice { get; set; }
        public decimal TotalExpenses { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? EditDate { get; set; }

        public string TotalExpensesDisplay => $"{TotalExpenses} руб.";

        public BillItem()
        {
            MeterReading = new MeterReadingItem();
        }

        public object Clone()
        {
            var clone = this.MemberwiseClone() as BillItem;
            clone.MeterReading = this.MeterReading.Clone() as MeterReadingItem;
            return clone;
        }
    }
}
