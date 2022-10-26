using Core.ExpenseEstimator;
using Infrastructure.ExpenseEstimator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Tests.ExpenseEstimator
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
