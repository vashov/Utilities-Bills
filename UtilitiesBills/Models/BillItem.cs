using SQLite;
using System;

namespace UtilitiesBills.Models
{
    [Table("Bills")]
    public class BillItem : ICloneable
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        
        [MaxLength(100)]
        public string Note { get; set; }

        /// <summary>
        /// Округленное значение горячей воды по счетчику
        /// </summary>
        public decimal HotWaterCounterBulkRounded { get; set; }

        /// <summary>
        /// Округленное значение холодной воды по счетчику
        /// </summary>
        public decimal ColdWaterCounterBulkRounded { get; set; }

        /// <summary>
        /// Округленное значение электричества по счетчику
        /// </summary>
        public decimal ElectricityCounterBulkRounded { get; set; }

        /// <summary>
        /// Объем использованной горячей воды
        /// </summary>
        public decimal HotWaterBulk { get; set; }

        /// <summary>
        /// Объем использованной холодной воды
        /// </summary>
        public decimal ColdWaterBulk { get; set; }

        /// <summary>
        /// Объем использованного электричества
        /// </summary>
        public decimal ElectricityBulk { get; set; }

        /// <summary>
        /// Объем отведенной воды
        /// </summary>
        public decimal WaterDisposalBulk { get; set; }

        public decimal HotWaterPrice { get; set; }
        public decimal ColdWaterPrice { get; set; }
        public decimal ElectricityPrice { get; set; }
        public decimal WaterDisposalPrice { get; set; }

        public decimal HotWaterExpenses { get; set; }
        public decimal ColdWaterExpenses { get; set; }
        public decimal ElectricityExpenses { get; set; }
        public decimal WaterDisposalExpenses { get; set; }
        public decimal TotalExpenses { get; set; }

        /// <summary>
        /// Дата снятия показаний с счётчиков
        /// </summary>
        public DateTime DateOfReading { get; set; }

        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// Дата последнего редактирования
        /// </summary>
        public DateTime? EditDate { get; set; }

        public string TotalExpensesDisplay => $"{TotalExpenses} руб.";

        public object Clone()
        {
           return MemberwiseClone();
        }
    }
}
