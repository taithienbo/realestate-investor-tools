using Core.Calculators;
using Core.Constants;
using Core.Dto;
using Infrastructure.Calculator;


namespace Infrastructure.Tests.Calculators
{
    public class ExpensesCalculatorTests
    {
        private IExpensesCalculator _expenseCalculator;


        public ExpensesCalculatorTests()
        {
            _expenseCalculator = new ExpensesCalculator(new MortgageCalculator(),
                new PropertyTaxCalculator(),
                new HomeOwnerInsuranceCalculator(),
                new CapExCalculator());
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
                PropertyTax = 11062.5 / 12,
                HomeOwnerInsurance = 184.37
            };
            // act 
            var actualExpenseDetail = _expenseCalculator.CalculateExpenses(listingDetail, loanDetail);
            Assert.NotNull(actualExpenseDetail);
            Assert.Equal(expectedExpenseDetail.Mortgage, actualExpenseDetail.Mortgage, 0);
            Assert.Equal(expectedExpenseDetail.PropertyTax, actualExpenseDetail.PropertyTax, 0);
            Assert.Equal(expectedExpenseDetail.HomeOwnerInsurance, actualExpenseDetail.HomeOwnerInsurance, 0);
        }

        [Fact]
        public void CalculateExpenses_Test2()
        {
            // arrange
            var listingDetail = new ListingDetail()
            {
                ListingPrice = 606000,
                YearBuilt = 1950
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
                PropertyTax = 7575 / 12,
                HomeOwnerInsurance = 126.25, // listingPrice * .25 / 100 / 12; 
                CapitalExpenditures = 173 // (base amount, which is .20% of listing price) + property age 
            };
            // act 
            var actualExpenseDetail = _expenseCalculator.CalculateExpenses(listingDetail, loanDetail);
            Assert.NotNull(actualExpenseDetail);
            Assert.Equal(expectedExpenseDetail.Mortgage, actualExpenseDetail.Mortgage, 0);
            Assert.Equal(expectedExpenseDetail.PropertyTax, actualExpenseDetail.PropertyTax, 0);
            Assert.Equal(expectedExpenseDetail.HomeOwnerInsurance, actualExpenseDetail.HomeOwnerInsurance, 0);
            Assert.Equal(expectedExpenseDetail.CapitalExpenditures, actualExpenseDetail.CapitalExpenditures, 0);
        }
    }
}
