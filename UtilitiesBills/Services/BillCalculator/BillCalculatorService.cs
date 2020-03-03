using System;

namespace UtilitiesBills.Services.BillCalculator
{
    public class BillCalculatorService : IBillCalculatorService
    {
        public decimal CalcBulkExpense(decimal bulk, decimal price)
        {
            return bulk * price;
        }

        public decimal CalcBulk(decimal currentBulk, decimal prevBulk)
        {
            return currentBulk - prevBulk; 
        }

        public decimal RoundBulkValue(decimal bulk)
        {
            return Math.Round(bulk);
        }

        public decimal RoundSum(decimal sum)
        {
            return Math.Round(sum);
        }
    }
}
