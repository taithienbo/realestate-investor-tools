using Core.Calculator;
using Core.Constants;
using Core.Dto;
using Core.Listing;
using Infrastructure.Calculator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Tests.Calculators
{
    public class ExpenseCalculatorTests
    {
        private IExpenseCalculator _expenseCalculator;

        public ExpenseCalculatorTests()
        {
            _expenseCalculator = new ExpenseCalculator();
        }
        [Fact]
        public void CalculateExpenses()
        {
            // arrange
            var listingDetail = new ListingDetail()
            {
                ListingPrice = 885000,
            };
            var loanDetail = new LoanDetail()
            {
                DownPaymentPercent = 25,
                InterestRate = 7.112,
                LoanProgram = LoanProgram.ThirtyYearFixed
            };
            var expectedExpenseDetail = new ExpenseDetail()
            {
                Mortgage = 4466,
                PropertyTax = 921875
            };
            // act 
            var actualExpenseDetail = _expenseCalculator.CalculateExpenses(listingDetail, loanDetail);
            Assert.NotNull(actualExpenseDetail);

        }
    }
}
