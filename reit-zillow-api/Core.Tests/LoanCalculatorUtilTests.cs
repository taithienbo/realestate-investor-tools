using Core.Constants;
using System.IO.Pipes;

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

        [Fact]
        public void CalculateCapRate()
        {
            // arrange 
            const double propertyPrice = 100000;
            const double monthlyRentalIncome = 1000;
            var monthlyIncomes = new Dictionary<string, double>()
            {
                { nameof(CommonIncomeType.Rental), monthlyRentalIncome }
            };
            // act 
            var capRate = Calculators.CalculateCapRate(propertyPrice, monthlyIncomes);
            // assert
            double expectedCapRatePercentage = 12;
            Assert.Equal(expectedCapRatePercentage, capRate, 0);
        }

        [Fact]
        public void CalculateDebtServiceCoverageRatio()
        {
            // arrange 
            const double MortgageAndInterest = 200;
            const double MiscExpenses = 300;
            const double MonthlyRental = 1000;

            var monthlyIncomes = new Dictionary<string, double>()
            {
                { nameof(CommonIncomeType.Rental), MonthlyRental }
            };

            var monthlyExpenses = new Dictionary<string, double>()
            {
                { nameof(CommonExpenseType.Mortgage), MortgageAndInterest },
                { nameof(CommonExpenseType.Misc), MiscExpenses }
            };

            // act 
            var debtServiceCoverageRatio = Calculators.CalculateDebtServiceCoverageRatio(monthlyIncomes, monthlyExpenses);

            // assert 
            var expectedNetOperatingIncome = (MonthlyRental * 12) - (MiscExpenses * 12);
            var expectedMortgageAndInterestDebtYearly = MortgageAndInterest * 12;
            var expectedDebtServiceCoverageRatio = expectedNetOperatingIncome / expectedMortgageAndInterestDebtYearly;
            Assert.Equal(expectedDebtServiceCoverageRatio, debtServiceCoverageRatio);
        }

        [Fact]
        public void CalculateCashFlow()
        {
            // arrange 
            const double RentalIncome = 3000;
            const double HazardInsuranceExpense = 200;
            const double CapitalExpenditures = 400;
            var incomes = new Dictionary<string, double>()
            {
                { nameof(CommonIncomeType.Rental), RentalIncome}
            };
            var expenses = new Dictionary<string, double>()
            {
                { nameof(CommonExpenseType.HomeOwnerInsurance), HazardInsuranceExpense },
                { nameof(CommonExpenseType.CapitalExpenditures), CapitalExpenditures }
            };
            // act
            var cashFlow = Calculators.CalculateCashFlow(incomes, expenses);
            var expectedCashFlow = 2400;
            Assert.Equal(expectedCashFlow, cashFlow, 0);
        }

        [Fact]
        public void CalculateCashOnCashReturn()
        {
            // arrange 
            const double MonthlyRentalIncome = 2700;
            const double MiscExpenses = 2600;
            const double DownPayment = 45000;
            const double ClosingCost = 5000;
            var incomes = new Dictionary<string, double>()
            {
                { nameof(CommonIncomeType.Rental), MonthlyRentalIncome }
            };
            var expenses = new Dictionary<string, double>()
            {
                { nameof(CommonExpenseType.Misc), MiscExpenses }
            };
            var totalInvestment = DownPayment + ClosingCost;

            // act 
            var cashOnCashReturn = Calculators.CalculateCashOnCashReturn(incomes, expenses, totalInvestment);
            // assert 
            //(Annual cash flow / TotalInvestment) * 100
            // ((MonthlyRentalIncome * 12) - (MiscExpenses * 12))/ (OutOfPocketInvestmentCost + ClosingCost) * 100
            // (32400 - 31200) / 50000 * 100 = 2.4
            var expectedCashOnCashReturn = 2.4;
            Assert.Equal(expectedCashOnCashReturn, cashOnCashReturn, 0);
        }

        [Fact]
        public void CalculateDownPayment()
        {
            // arrange 
            const double ListingPrice = 565000;
            const double DownPaymentPercent = 25;
            // act
            var downPayment = Calculators.CalculateDownPayment(ListingPrice, DownPaymentPercent);
            var expectedDownPayment = 141250;// DownPaymentPercent / 100 * ListingPrice
            // assert
            Assert.Equal(expectedDownPayment, downPayment, 0);
        }
    }
}
