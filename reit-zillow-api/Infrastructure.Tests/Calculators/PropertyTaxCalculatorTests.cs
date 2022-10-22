using Core.Calculators;
using Infrastructure.Calculator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Tests.Calculators
{
    public class PropertyTaxCalculatorTests
    {
        private IPropertyTaxCalculator _monthlyPropertyTaxCalculator;

        public PropertyTaxCalculatorTests()
        {
            _monthlyPropertyTaxCalculator = new PropertyTaxCalculator();
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
