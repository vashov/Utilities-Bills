using System;

namespace UtilitiesBills.Services.BillCalculator
{
    public class BillCalculatorService : IBillCalculatorService
    {
        public decimal CalcBulkExpense(decimal bulk, decimal price)
        {
            return bulk * price;
        }

        public decimal CalcBulk(decimal currentCounterBulk, decimal prevCounterBulk)
        {
            return currentCounterBulk - prevCounterBulk; 
        }

        public decimal RoundCounterBulk(decimal counterBulk)
        {
            return Math.Round(counterBulk, MidpointRounding.AwayFromZero);
        }

        public decimal RoundSum(decimal sum)
        {
            return Math.Round(sum, MidpointRounding.AwayFromZero);
        }
    }
}
