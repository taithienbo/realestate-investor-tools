using Core.ExpenseEstimator;
using Core.Constants;
using Infrastructure.ExpenseEstimator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Tests.Calculators
{
    public class CapExpenseEstimatorTests
    {
        private ICapExExpenseEstimator _calculator;

        public CapExpenseEstimatorTests()
        {
            _calculator = new CapExEstimator();
        }

        [Fact]
        public void Calculate()
        {
            // arrange 
            var propertyAge = 10;
            var propertyValue = 600000;

            // act
            var actualEstimatedCapExMonthlyAmount = _calculator.CalculateEstimatedMonthlyCapEx(propertyValue, propertyAge);
            // assert
            var expectedMonthlyBasedAmount = .20 / 100 * propertyValue / 12;
            var expectedAdditionalMonthlyAmountBasedOnAge = propertyAge;
            var estimatedCapExMonthlyAmount = expectedMonthlyBasedAmount + expectedAdditionalMonthlyAmountBasedOnAge;
            Assert.Equal(estimatedCapExMonthlyAmount, actualEstimatedCapExMonthlyAmount);
        }
    }
}
