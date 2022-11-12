using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Tests
{
    public class LoanCalculatorUtilTests
    {
        [Fact]
        public void CalculateLoanAmountGivenPropertyPriceAndDownPaymentPercentage()
        {
            //arrange 
            var propertyPrice = 595000;
            var downPaymentPercentage = 25;
            // act 
            var loanAmount = LoanCalculatorUtil.CalculateLoanAmount(propertyPrice, downPaymentPercentage);
            // assert
            var expectedLoanAmount = 446250;
            Assert.Equal(expectedLoanAmount, loanAmount);
        }
    }
}
