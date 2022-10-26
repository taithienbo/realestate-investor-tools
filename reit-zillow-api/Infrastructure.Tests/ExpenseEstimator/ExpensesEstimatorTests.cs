﻿using Core.ExpenseEstimator;
using Core.Constants;
using Core.Dto;
using Infrastructure.ExpenseEstimator;


namespace Infrastructure.Tests.Calculators
{
    public class ExpensesEstimatorTests
    {
        private IExpenseEstimator _expenseCalculator;


        public ExpensesEstimatorTests()
        {
            _expenseCalculator = new EspenseEstimator(new MortgageExpenseEstimator(),
                new PropertyTaxExpenseEstimator(),
                new HomeOwnerInsuranceExpenseEstimator(),
                new CapExEstimator(),
                new RepairExpenseEstimator(),
                new PropertyManagementExpenseEstimator());
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

            var expectedExpenseDetail = new ExpenseDetail()
            {
                Mortgage = 3143,
                PropertyTax = 7575 / 12,
                HomeOwnerInsurance = listingDetail.ListingPrice * HomeOwnerInsuranceCostAsPercentageOfPropertyValue / 100 / 12,
                CapitalExpenditures = CapExCostAsPercentageOfPropertyValue / 100 * listingDetail.ListingPrice / 12 + listingDetail.PropertyAge, // (base amount, which is .20% of listing price) + property age 
                Repairs = BaseMonthlyRepairCost + listingDetail.PropertyAge,
                PropertyManagement = PropertyManagementCostAsPercentageOfRentAmount / 100 * rentAmount
            };
            // act 
            var actualExpenseDetail = _expenseCalculator.CalculateExpenses(listingDetail, loanDetail, rentAmount);
            Assert.NotNull(actualExpenseDetail);
            Assert.Equal(expectedExpenseDetail.Mortgage, actualExpenseDetail.Mortgage, 0);
            Assert.Equal(expectedExpenseDetail.PropertyTax, actualExpenseDetail.PropertyTax, 0);
            Assert.Equal(expectedExpenseDetail.HomeOwnerInsurance, actualExpenseDetail.HomeOwnerInsurance, 0);
            Assert.Equal(expectedExpenseDetail.CapitalExpenditures, actualExpenseDetail.CapitalExpenditures, 0);
            Assert.Equal(expectedExpenseDetail.Repairs, actualExpenseDetail.Repairs, 0);
            Assert.Equal(expectedExpenseDetail.PropertyManagement, actualExpenseDetail.PropertyManagement, 0);

        }
    }
}
