using Core.Expense;
using Core.Options;

namespace Infrastructure.Tests.Expense
{
    public class PropertyManagementExpenseEstimatorTests
    {
        private IPropertyManagementExpenseEstimator _estimator;
        private readonly AppOptions _appOptions;

        public PropertyManagementExpenseEstimatorTests()
        {
            _appOptions = new AppOptions()
            {
                BasePropertyManagementCostAsPercentageOfMonthlyRent = 5.00
            };

            _estimator = new PropertyManagementExpenseEstimator(_appOptions);
        }

        [Fact]
        public void EstimateMonthlyAmountAsPercentageOfRentAmount()
        {
            // arrange 
            var rentAmount = 2700;
            double propertyManagementCostAsPercentageOfPropertyValue = _appOptions.BasePropertyManagementCostAsPercentageOfMonthlyRent;
            // act
            double propertyMangementMonthlyCost = _estimator.EstimateMonthlyAmount(rentAmount);
            // assert
            double expectedPropertyManagementMonthlyCost = propertyManagementCostAsPercentageOfPropertyValue / 100 * rentAmount;
            Assert.Equal(expectedPropertyManagementMonthlyCost, propertyMangementMonthlyCost);
        }
    }
}
