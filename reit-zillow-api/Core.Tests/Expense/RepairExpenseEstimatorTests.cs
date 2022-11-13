using Core.Expense;

namespace Infrastructure.Tests.Expense
{
    public class RepairExpenseEstimatorTests
    {
        private IRepairExpenseEstimator _estimator;

        public RepairExpenseEstimatorTests()
        {
            _estimator = new RepairExpenseEstimator();
        }

        [Fact]
        public void EstimateMonthlyAmount()
        {
            // arrange
            var propertyAge = 30;
            // act 
            var actualMonthlyAmount = _estimator.EstimateMonthlyAmount(propertyAge);
            // assert
            const int ExpectedMonthlyBaseAmount = 110;
            var expectedMonthlyAmount = ExpectedMonthlyBaseAmount + propertyAge;
            Assert.Equal(expectedMonthlyAmount, actualMonthlyAmount, 0);

        }
    }
}
