using Core.Expense;
using Core.Constants;
using Core.Dto;
using Infrastructure.Expense;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

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
        public void EstimateExpenses()
        {
            // arrange 
            var estimateExpenseRequest = new EstimateExpensesRequest()
            {
                PropertyValue = 606000,
                PropertyAge = 72,
                DownPaymentPercent = 20,
                InterestRate = 6.746,
                LoanProgram = LoanProgram.ThirtyYearFixed,
                RentAmount = 2700
            };

            const int BaseMonthlyRepairCost = 110;
            const double HomeOwnerInsuranceCostAsPercentageOfPropertyValue = .25;
            const double CapExCostAsPercentageOfPropertyValue = .20;
            const double PropertyManagementCostAsPercentageOfRentAmount = 5.00;
            const double MiscMonthlyExpense = 100;

            var expectedExpenseDetail = new ExpenseDetail()
            {
                Mortgage = 3143,
                PropertyTax = 7575 / 12,
                HomeOwnerInsurance = estimateExpenseRequest.PropertyValue * HomeOwnerInsuranceCostAsPercentageOfPropertyValue / 100 / 12,
                CapitalExpenditures = CapExCostAsPercentageOfPropertyValue / 100 * estimateExpenseRequest.PropertyValue / 12 + estimateExpenseRequest.PropertyAge, // (base amount, which is .20% of listing price) + property age 
                Repairs = BaseMonthlyRepairCost + estimateExpenseRequest.PropertyAge,
                PropertyManagement = PropertyManagementCostAsPercentageOfRentAmount / 100 * estimateExpenseRequest.RentAmount,
                Misc = MiscMonthlyExpense
            };
            // act 
            var actualExpenseDetail = _expenseEstimator.EstimateExpenses(estimateExpenseRequest);
            // assert
            Assert.NotNull(actualExpenseDetail);
            Assert.Equal(expectedExpenseDetail.Mortgage, actualExpenseDetail.Mortgage, 0);
            Assert.Equal(expectedExpenseDetail.PropertyTax, actualExpenseDetail.PropertyTax, 0);
            Assert.Equal(expectedExpenseDetail.HomeOwnerInsurance, actualExpenseDetail.HomeOwnerInsurance, 0);
            Assert.Equal(expectedExpenseDetail.CapitalExpenditures, actualExpenseDetail.CapitalExpenditures, 0);
            Assert.Equal(expectedExpenseDetail.Repairs, actualExpenseDetail.Repairs, 0);
            Assert.Equal(expectedExpenseDetail.PropertyManagement, actualExpenseDetail.PropertyManagement, 0);
            Assert.Equal(expectedExpenseDetail.Misc, actualExpenseDetail.Misc, 0);


            Assert.Equal((int)expectedExpenseDetail.Total, (int)actualExpenseDetail.Total);
        }

    }
}
