using Core.Expense;

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
