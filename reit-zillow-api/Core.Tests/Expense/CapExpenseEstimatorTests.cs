using Core.Expense;
using Core.Options;

namespace Infrastructure.Tests.Expense
{
    public class CapExpenseEstimatorTests
    {
        private ICapExExpenseEstimator _calculator;
        private readonly AppOptions _appOptions;

        public CapExpenseEstimatorTests()
        {
            _appOptions = new AppOptions()
            {
                BaseCapExPercentOfPropertyValue = .20
            };

            _calculator = new CapExExpenseEstimator(_appOptions);
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
