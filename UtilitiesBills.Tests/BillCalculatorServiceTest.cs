using UtilitiesBills.Services.BillCalculator;
using Xunit;

namespace UtilitiesBills.Tests
{
    public class BillCalculatorServiceTest
    {
        [Theory]
        [InlineData(0,   0)]
        [InlineData(3,   3)]
        [InlineData(3.4, 3)]
        [InlineData(3.5, 4)]
        [InlineData(3.6, 4)]
        [InlineData(4.4, 4)]
        [InlineData(4.5, 5)]
        [InlineData(4.6, 5)]
        public void RoundCounterBulkTest(decimal bulk, decimal expectedBulkAfterRound)
        {
            // Arrange
            var calculator = new BillCalculatorService();

            // Act
            decimal resultBulkAfterRound = calculator.RoundCounterBulk(bulk);

            // Assert
            Assert.Equal(expectedBulkAfterRound, resultBulkAfterRound);
        }

        [Theory]
        [InlineData(0,   0)]
        [InlineData(3,   3)]
        [InlineData(3.4, 3)]
        [InlineData(3.5, 4)]
        [InlineData(3.6, 4)]
        [InlineData(4.4, 4)]
        [InlineData(4.5, 5)]
        [InlineData(4.6, 5)]
        public void RoundSumTest(decimal sumBeforeRound, decimal expectedSumAfterRound)
        {
            // Arrange
            var calculator = new BillCalculatorService();

            // Act
            decimal resultSum = calculator.RoundSum(sumBeforeRound);

            // Assert
            Assert.Equal(expectedSumAfterRound, resultSum);
        }

        [Theory]
        [InlineData(0,   0,   0)]
        [InlineData(3,   0,   0)]
        [InlineData(0,   3,   0)]
        [InlineData(2,   3,   6)]
        [InlineData(3.4, 2,   6.8)]
        [InlineData(3.5, 2.6, 9.1)]
        public void CalcBulkExpenseTest(decimal bulk, decimal price, decimal expectExpenseResult)
        {
            // Arrange
            var calculator = new BillCalculatorService();

            // Act
            decimal expenseResult = calculator.CalcBulkExpense(bulk, price);

            // Assert
            Assert.Equal(expectExpenseResult, expenseResult);
        }

        [Theory]
        [InlineData(0,   0,  0)]
        [InlineData(3,   2,  1)]
        [InlineData(3,   3,  0)]
        [InlineData(3.4, 2,  1.4)]
        [InlineData(3.5, 2,  1.5)]
        [InlineData(2,   3, -1)]
        public void CalcBulkTest(decimal currBulk, decimal prevBulk, decimal expectedBulkResult)
        {
            // Arrange
            var calculator  = new BillCalculatorService();

            // Act
            decimal bulkResult = calculator.CalcBulk(currBulk, prevBulk);

            // Assert
            Assert.Equal(expectedBulkResult, bulkResult);
        }
    }
}
