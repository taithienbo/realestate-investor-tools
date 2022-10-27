using Core.Expense;
using Infrastructure.Expense;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Tests.Expense
{
    public class MiscExpenseEstimatorTests
    {
        private IMiscExpenseEstimator _estimator;

        public MiscExpenseEstimatorTests()
        {
            _estimator = new MiscExpenseEstimator();
        }

        [Fact]
        public void EstimateMonthlyAmount()
        {
            // arrange
            var expectedMiscMonthlyAmount = 100;
            // act 
            var actualMiscMonthlyAmount = _estimator.EstimateMonthlyAmount();
            // assert
            Assert.Equal(expectedMiscMonthlyAmount, actualMiscMonthlyAmount, 0);
        }
    }
}
