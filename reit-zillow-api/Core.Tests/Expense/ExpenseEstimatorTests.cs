using Core.Constants;
using Core.Dto;
using Core.Expense;
using Core.Loan;
using Core.Options;

namespace Infrastructure.Tests.Expense
{
    public class ExpenseEstimatorTests
    {
        private IExpenseEstimator _expenseEstimator;
        private readonly AppOptions _appOptions;

        public ExpenseEstimatorTests()
        {
            _appOptions = new AppOptions()
            {
                BaseCapExPercentOfPropertyValue = .20,
                BaseMiscExpenseMonthlyAmount = 100,
                BaseRepairMonthlyAmount = 110,
                BaseHomeOwnerInsurancePercentageOfPropertyValue = .25,
                BasePropertyManagementCostAsPercentageOfMonthlyRent = 5.00
            };
            _expenseEstimator = new ExpenseEstimator(new MortgageExpenseEstimator(),
                new PropertyTaxExpenseEstimator(),
                new HomeOwnerInsuranceExpenseEstimator(_appOptions),
                new CapExExpenseEstimator(_appOptions),
                new RepairExpenseEstimator(_appOptions),
                new PropertyManagementExpenseEstimator(_appOptions),
                new MiscExpenseEstimator(_appOptions));
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
                RentAmount = 2700,
                HoaFee = 200
            };

            double baseMonthlyRepairCost = _appOptions.BaseRepairMonthlyAmount;
            double homeOwnerInsuranceCostAsPercentageOfPropertyValue = _appOptions.BaseHomeOwnerInsurancePercentageOfPropertyValue;
            double capExCostAsPercentageOfPropertyValue = _appOptions.BaseCapExPercentOfPropertyValue;
            double propertyManagementCostAsPercentageOfRentAmount = _appOptions.BasePropertyManagementCostAsPercentageOfMonthlyRent;
            double miscMonthlyExpense = _appOptions.BaseMiscExpenseMonthlyAmount;

            var expectedExpenses = new Dictionary<string, double>()
            {
                { nameof(CommonExpenseType.Mortgage), 3143 },
                { nameof(CommonExpenseType.PropertyTax), 7575 / 12 },
                { nameof(CommonExpenseType.HomeOwnerInsurance),
                estimateExpenseRequest.PropertyValue * homeOwnerInsuranceCostAsPercentageOfPropertyValue / 100 / 12
                },
                { nameof(CommonExpenseType.CapitalExpenditures),
                capExCostAsPercentageOfPropertyValue / 100 * estimateExpenseRequest.PropertyValue / 12 + estimateExpenseRequest.PropertyAge // (base amount, which is .20% of listing price) + property age ) }
                },
                { nameof(CommonExpenseType.Repairs), baseMonthlyRepairCost + estimateExpenseRequest.PropertyAge
                },
                {
                    nameof(CommonExpenseType.PropertyManagement),
                    propertyManagementCostAsPercentageOfRentAmount / 100 * estimateExpenseRequest.RentAmount
                },
                {
                    nameof(CommonExpenseType.HoaFee),       estimateExpenseRequest.HoaFee
                },
                {
                    nameof(CommonExpenseType.Misc),
                    miscMonthlyExpense
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
