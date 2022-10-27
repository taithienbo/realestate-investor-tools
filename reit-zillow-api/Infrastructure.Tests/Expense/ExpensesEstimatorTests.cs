using Core.Expense;
using Core.Constants;
using Core.Dto;
using Infrastructure.Expense;

namespace Infrastructure.Tests.Expense
{
    public class ExpensesEstimatorTests
    {
        private IExpenseEstimator _expenseEstimator;


        public ExpensesEstimatorTests()
        {
            _expenseEstimator = new ExpenseEstimator(new MortgageExpenseEstimator(),
                new PropertyTaxExpenseEstimator(),
                new HomeOwnerInsuranceExpenseEstimator(),
                new CapExExpenseEstimator(),
                new RepairExpenseEstimator(),
                new PropertyManagementExpenseEstimator(),
                new MiscExpenseEstimator());
        }

        [Fact]
        public void CalculateExpenses()
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

            var rentAmount = 2700;

            const int BaseMonthlyRepairCost = 110;
            const double HomeOwnerInsuranceCostAsPercentageOfPropertyValue = .25;
            const double CapExCostAsPercentageOfPropertyValue = .20;
            const double PropertyManagementCostAsPercentageOfRentAmount = 5.00;
            const double MiscMonthlyExpense = 100;

            var expectedExpenseDetail = new ExpenseDetail()
            {
                Mortgage = 3143,
                PropertyTax = 7575 / 12,
                HomeOwnerInsurance = listingDetail.ListingPrice * HomeOwnerInsuranceCostAsPercentageOfPropertyValue / 100 / 12,
                CapitalExpenditures = CapExCostAsPercentageOfPropertyValue / 100 * listingDetail.ListingPrice / 12 + listingDetail.PropertyAge, // (base amount, which is .20% of listing price) + property age 
                Repairs = BaseMonthlyRepairCost + listingDetail.PropertyAge,
                PropertyManagement = PropertyManagementCostAsPercentageOfRentAmount / 100 * rentAmount,
                Misc = MiscMonthlyExpense
            };
            // act 
            var actualExpenseDetail = _expenseEstimator.CalculateExpenses(listingDetail, loanDetail, rentAmount);
            // assert
            Assert.NotNull(actualExpenseDetail);
            Assert.Equal(expectedExpenseDetail.Mortgage, actualExpenseDetail.Mortgage, 0);
            Assert.Equal(expectedExpenseDetail.PropertyTax, actualExpenseDetail.PropertyTax, 0);
            Assert.Equal(expectedExpenseDetail.HomeOwnerInsurance, actualExpenseDetail.HomeOwnerInsurance, 0);
            Assert.Equal(expectedExpenseDetail.CapitalExpenditures, actualExpenseDetail.CapitalExpenditures, 0);
            Assert.Equal(expectedExpenseDetail.Repairs, actualExpenseDetail.Repairs, 0);
            Assert.Equal(expectedExpenseDetail.PropertyManagement, actualExpenseDetail.PropertyManagement, 0);
            Assert.Equal(expectedExpenseDetail.Misc, actualExpenseDetail.Misc, 0);
            var expectedTotalExpenses = expectedExpenseDetail.Mortgage + expectedExpenseDetail.PropertyTax
                + expectedExpenseDetail.HomeOwnerInsurance + expectedExpenseDetail.CapitalExpenditures
                + expectedExpenseDetail.Repairs + expectedExpenseDetail.PropertyManagement
                + expectedExpenseDetail.Misc;

            Assert.Equal(expectedExpenseDetail.Total, actualExpenseDetail.Total, 0);

        }
    }
}
