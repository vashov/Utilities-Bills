namespace UtilitiesBills.Services.BillCalculator
{
    public interface IBillCalculatorService
    {
        decimal RoundCounterBulk(decimal counterBulk);
        decimal RoundSum(decimal sum);
        decimal CalcBulkExpense(decimal bulk, decimal price);
        decimal CalcBulk(decimal currentCounterBulk, decimal prevCounterBulk);
    }
}
