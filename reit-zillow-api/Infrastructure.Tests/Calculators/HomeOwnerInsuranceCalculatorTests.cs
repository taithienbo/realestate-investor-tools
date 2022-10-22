using Core.Calculators;
using Infrastructure.Calculator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Tests.Calculators
{

    public class HomeOwnerInsuranceCalculatorTests
    {
        private IHomeOwnerInsuranceCalculator _homeOwnerInsuranceCalculator;

        public HomeOwnerInsuranceCalculatorTests()
        {
            _homeOwnerInsuranceCalculator = new HomeOwnerInsuranceCalculator();
        }

        [Fact]
        public void CalculateEstimatedMonthlyAmount()
        {
            // arrange 
            var listingPrice = 600000;
            var expectedMonthlyAmount = 125; // listingPrice * .25 / 100 / 12;
            var actualMonthlyAmount = _homeOwnerInsuranceCalculator
                .CalculateMonthlyAmount(listingPrice);
            Assert.Equal(expectedMonthlyAmount, actualMonthlyAmount, 0);
        }
    }
}
