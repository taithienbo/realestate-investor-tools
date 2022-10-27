using Core.Expense;
using Infrastructure.Expense;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Tests.Expense
{
    public class PropertyTaxExpenseEstimatorTests
    {
        private IPropertyTaxExpenseEstimator _monthlyPropertyTaxCalculator;

        public PropertyTaxExpenseEstimatorTests()
        {
            _monthlyPropertyTaxCalculator = new PropertyTaxExpenseEstimator();
        }

        [Fact]
        public void CalculateMonthlyPropertyTax()
        {
            // arrange 
            var purchasePrice = 606000;
            var expectedMonthlyPropertyTax = 631.25;
            var estimatedPropertyTaxRate = 1.25;
            // act 
            var actualMonthlyPropertyTax = _monthlyPropertyTaxCalculator.Calculate(purchasePrice, estimatedPropertyTaxRate);
            Assert.Equal(expectedMonthlyPropertyTax, actualMonthlyPropertyTax, 0);
        }
    }
}
