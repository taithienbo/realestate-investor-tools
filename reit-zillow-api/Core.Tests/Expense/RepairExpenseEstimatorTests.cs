using Core.Expense;
using Core.Options;

namespace Infrastructure.Tests.Expense
{
    public class RepairExpenseEstimatorTests
    {
        private IRepairExpenseEstimator _estimator;
        private readonly AppOptions _appOptions;

        public RepairExpenseEstimatorTests()
        {
            _appOptions = new AppOptions()
            {
                BaseRepairMonthlyAmount = 110
            };

            _estimator = new RepairExpenseEstimator(_appOptions);
        }

        [Fact]
        public void EstimateMonthlyAmount()
        {
            // arrange
            var propertyAge = 30;
            // act 
            var actualMonthlyAmount = _estimator.EstimateMonthlyAmount(propertyAge);
            // assert
            double expectedMonthlyBaseAmount = _appOptions.BaseRepairMonthlyAmount;
            var expectedMonthlyAmount = expectedMonthlyBaseAmount + propertyAge;
            Assert.Equal(expectedMonthlyAmount, actualMonthlyAmount, 0);

        }
    }
}
