namespace UtilitiesBills.Services.BillCalculator
{
    public interface IBillCalculatorService
    {
        decimal RoundBulk(decimal bulk);
        decimal RoundSum(decimal sum);
        decimal CalcBulkExpense(decimal bulk, decimal price);
        decimal CalcNetBulk(decimal currentBulk, decimal prevBulk);
    }
}
