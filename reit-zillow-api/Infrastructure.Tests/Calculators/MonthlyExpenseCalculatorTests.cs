using Core.Calculators;
using Core.Constants;
using Core.Dto;
using Infrastructure.Calculator;


namespace Infrastructure.Tests.Calculators
{
    public class MonthlyExpenseCalculatorTests
    {
        private IMonthlyExpenseCalculator _expenseCalculator;

        public MonthlyExpenseCalculatorTests()
        {
            _expenseCalculator = new MonthlyExpenseCalculator();
        }
        [Fact]
        public void CalculateExpenses_Test1()
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
                PropertyTax = 11062.5 / 12
            };
            // act 
            var actualExpenseDetail = _expenseCalculator.CalculateExpenses(listingDetail, loanDetail);
            Assert.NotNull(actualExpenseDetail);
            Assert.Equal(expectedExpenseDetail.Mortgage, actualExpenseDetail.Mortgage);
            Assert.Equal(expectedExpenseDetail.PropertyTax, actualExpenseDetail.PropertyTax);
        }

        [Fact]
        public void CalculateExpenses_Test2()
        {
            // arrange
            var listingDetail = new ListingDetail()
            {
                ListingPrice = 606000,
            };
            var loanDetail = new LoanDetail()
            {
                DownPaymentPercent = 20,
                InterestRate = 6.746,
                LoanProgram = LoanProgram.ThirtyYearFixed
            };
            var expectedExpenseDetail = new ExpenseDetail()
            {
                Mortgage = 3143,
                PropertyTax = 7575 / 12
            };
            // act 
            var actualExpenseDetail = _expenseCalculator.CalculateExpenses(listingDetail, loanDetail);
            Assert.NotNull(actualExpenseDetail);
            Assert.Equal(expectedExpenseDetail.Mortgage, actualExpenseDetail.Mortgage);
            Assert.Equal(expectedExpenseDetail.PropertyTax, actualExpenseDetail.PropertyTax);
        }
    }
}
