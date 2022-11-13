using Core.Constants;

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
            var loanAmount = Calculators.CalculateLoanAmount(propertyPrice, downPaymentPercentage);
            // assert
            var expectedLoanAmount = 446250;
            Assert.Equal(expectedLoanAmount, loanAmount);
        }

        [Fact]
        public void CalculateNetOperatingIncome()
        {
            // arrange 
            var expenses = new Dictionary<string, double>();
            const double Mortgage = 500;
            const double OtherExpenses = 400;
            const double rentalIncome = 700;
            expenses.Add(nameof(CommonExpenseType.Misc), OtherExpenses);
            expenses.Add(nameof(CommonExpenseType.Mortgage), Mortgage);

            var incomes = new Dictionary<string, double>();
            incomes.Add(nameof(CommonIncomeType.Rental), rentalIncome);
            // act 
            var noi = Calculators.CalculateNetOperatingIncome(incomes, expenses);

            //NOI = The profit you’ve received in a year, excluding your mortgage payment. 
            var expectedNoi = (rentalIncome * 12) - (OtherExpenses * 12);
            Assert.Equal(expectedNoi, noi);

        }
    }
}
