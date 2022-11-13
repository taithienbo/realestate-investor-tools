using Core.Expense;

namespace Infrastructure.Tests.Expense
{
    public class PropertyManagementExpenseEstimatorTests
    {
        private IPropertyManagementExpenseEstimator _estimator;

        public PropertyManagementExpenseEstimatorTests()
        {
            _estimator = new PropertyManagementExpenseEstimator();
        }

        [Fact]
        public void EstimateMonthlyAmountAsPercentageOfRentAmount()
        {
            // arrange 
            var rentAmount = 2700;
            const double PropertyManagementCostAsPercentageOfPropertyValue = 5.00;
            // act
            double propertyMangementMonthlyCost = _estimator.EstimateMonthlyAmount(rentAmount);
            // assert
            double expectedPropertyManagementMonthlyCost = PropertyManagementCostAsPercentageOfPropertyValue / 100 * rentAmount;
            Assert.Equal(expectedPropertyManagementMonthlyCost, propertyMangementMonthlyCost);
        }
    }
}
