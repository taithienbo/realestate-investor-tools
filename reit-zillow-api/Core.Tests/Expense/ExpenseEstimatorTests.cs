using Core.Constants;
using Core.Dto;
using Core.Expense;

namespace Infrastructure.Tests.Expense
{
    public class ExpenseEstimatorTests
    {
        private IExpenseEstimator _expenseEstimator;


        public ExpenseEstimatorTests()
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
                LoanProgram = LoanProgram.ThirtyYearFixed.ToString(),
                RentAmount = 2700
            };

            const int BaseMonthlyRepairCost = 110;
            const double HomeOwnerInsuranceCostAsPercentageOfPropertyValue = .25;
            const double CapExCostAsPercentageOfPropertyValue = .20;
            const double PropertyManagementCostAsPercentageOfRentAmount = 5.00;
            const double MiscMonthlyExpense = 100;

            var expectedExpenses = new Dictionary<string, double>()
            {
                { nameof(CommonExpenseType.Mortgage), 3143 },
                { nameof(CommonExpenseType.PropertyTax), 7575 / 12 },
                { nameof(CommonExpenseType.HomeOwnerInsurance),
                estimateExpenseRequest.PropertyValue * HomeOwnerInsuranceCostAsPercentageOfPropertyValue / 100 / 12
                },
                { nameof(CommonExpenseType.CapitalExpenditures),
                CapExCostAsPercentageOfPropertyValue / 100 * estimateExpenseRequest.PropertyValue / 12 + estimateExpenseRequest.PropertyAge // (base amount, which is .20% of listing price) + property age ) }
                },
                { nameof(CommonExpenseType.Repairs), BaseMonthlyRepairCost + estimateExpenseRequest.PropertyAge
                },
                {
                    nameof(CommonExpenseType.PropertyManagement),
                    PropertyManagementCostAsPercentageOfRentAmount / 100 * estimateExpenseRequest.RentAmount
                },
                {
                    nameof(CommonExpenseType.Misc),
                    MiscMonthlyExpense
                }
            };


            // act 
            var actualExpenses = _expenseEstimator.EstimateExpenses(estimateExpenseRequest);
            // assert
            Assert.NotNull(actualExpenses);
            foreach (var commonExpense in Enum.GetNames(typeof(CommonExpenseType)))
            {
                Assert.True(actualExpenses.ContainsKey(commonExpense));
                Assert.Equal(expectedExpenses[commonExpense], actualExpenses[commonExpense], 0);
            }
        }

    }
}
