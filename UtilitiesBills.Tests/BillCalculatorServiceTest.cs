using System.Collections.Generic;
using UtilitiesBills.Services.BillCalculator;
using Xunit;

namespace UtilitiesBills.Tests
{
    public class BillCalculatorServiceTest
    {
        [Fact]
        public void RoundCounterBulkTest()
        {
            // Arrange
            var calculator = new BillCalculatorService();
            var counterBulks = new List<decimal> { 0, 3, 3.4M, 3.45M, 3.5M, 3.6M, 4, 4.4M, 4.45M, 4.5M, 4.6M };
            var expectBulks  = new List<decimal> { 0, 3, 3,    4,     4,    4,    4, 4,    5,     5,    5 };
            var resultBulks = new List<decimal>();

            // Act
            foreach (decimal bulk in counterBulks)
            {
                resultBulks.Add(calculator.RoundCounterBulk(bulk));
            }

            // Assert
            for (int i = 0; i < expectBulks.Count; i++)
            {
                Assert.Equal(expectBulks[i], resultBulks[i]);
            }
        }

        [Fact]
        public void RoundSumTest()
        {
            // Arrange
            var calculator = new BillCalculatorService();
            var beforeSums = new List<decimal> { 0, 3, 3.4M, 3.5M, 3.6M, 4, 4.4M, 4.5M, 4.6M };
            var expectSums = new List<decimal> { 0, 3, 3,    4,    4,    4, 4,    5,    5 };
            var resultSums = new List<decimal>();

            // Act
            foreach (decimal sum in beforeSums)
            {
                resultSums.Add(calculator.RoundSum(sum));
            }

            // Assert
            for (int i = 0; i < expectSums.Count; i++)
            {
                Assert.Equal(expectSums[i], resultSums[i]);
            }
        }

        [Fact]
        public void CalcBulkExpenseTest()
        {
            // Arrange
            var calculator = new BillCalculatorService();
            var bulks  = new List<decimal> { 0, 3, 0, 3.4M, 3.5M, 2 };
            var prices = new List<decimal> { 0, 0, 3, 2,    2.6M, 3 };
            var expect = new List<decimal> { 0, 0, 0, 6.8M, 9.1M, 6 };
            var resultExpenses = new List<decimal>();

            // Act
            for (int i = 0; i < bulks.Count; i++)
            {
                resultExpenses.Add(calculator.CalcBulkExpense(bulks[i], prices[i]));
            }

            // Assert
            for (int i = 0; i < expect.Count; i++)
            {
                Assert.Equal(expect[i], resultExpenses[i]);
            }
        }

        [Fact]
        public void CalcBulkTest()
        {
            // Arrange
            var calculator  = new BillCalculatorService();
            var currBulks   = new List<decimal> { 0, 3, 3, 3.4M, 3.5M, 2 };
            var prevBulks   = new List<decimal> { 0, 2, 3, 2,    2,    3 };
            var expectBulks = new List<decimal> { 0, 1, 0, 1.4M, 1.5M, -1 };
            var resultBulks = new List<decimal>();

            // Act
            for (int i = 0; i < currBulks.Count; i++)
            {
                resultBulks.Add(calculator.CalcBulk(currBulks[i], prevBulks[i]));
            }

            // Assert
            for (int i = 0; i < expectBulks.Count; i++)
            {
                Assert.Equal(expectBulks[i], resultBulks[i]);
            }
        }
    }
}
