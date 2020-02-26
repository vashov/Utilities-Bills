using System;

namespace UtilitiesBills.Models
{
    public class BillItem
    {
        public int Id { get; set; }
        public string Note { get; set; }
        public decimal HotWaterValue { get; set; }
        public decimal ColdWaterValue { get; set; }
        public decimal WaterDisposalValue { get; set; }
        public decimal ElectricityValue { get; set; }
        public decimal HotWaterPrice { get; set; }
        public decimal ColdWaterPrice { get; set; }
        public decimal ElectricityPrice { get; set; }
        public decimal WaterDisposalPrice { get; set; }
        public decimal TotalExpenses { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime EditDate { get; set; }

        public string TotalExpensesDisplay => $"{TotalExpenses} руб.";
    }
}
