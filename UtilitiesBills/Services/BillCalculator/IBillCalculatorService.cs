namespace UtilitiesBills.Services.BillCalculator
{
    public interface IBillCalculatorService
    {
        decimal RoundBulkValue(decimal bulkValue);
        decimal RoundSum(decimal sum);
        decimal CalcBulkExpense(decimal bulk, decimal price);
        decimal CalcBulk(decimal currentBulkValue, decimal prevBulkValue);
    }
}
