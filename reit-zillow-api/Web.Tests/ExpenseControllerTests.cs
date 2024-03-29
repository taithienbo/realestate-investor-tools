﻿using Core.Constants;
using Core.Dto;
using Core.Expense;
using Core.Income;
using Core.Interest;
using Core.Listing;
using Core.Options;
using Core.Zillow;
using Moq;
using reit_zillow_api.Controllers;

namespace Web.Tests
{
    public class ExpenseControllerTests
    {
        private readonly ExpenseController _expenseController;
        private readonly IExpenseEstimator _expenseEstimator;

        private readonly Mock<IExpenseService> _mockExpenseService;

        private readonly AppOptions _appOptions;

        public ExpenseControllerTests()
        {
            _appOptions = new AppOptions()
            {
                BaseCapExPercentOfPropertyValue = .20,
                BaseMiscExpenseMonthlyAmount = 100,
                BaseRepairMonthlyAmount = 110,
                BaseHomeOwnerInsurancePercentageOfPropertyValue = 0.25,
                BasePropertyManagementCostAsPercentageOfMonthlyRent = 5.00
            };

            _expenseEstimator = new ExpenseEstimator(new MortgageExpenseEstimator(),
                new PropertyTaxExpenseEstimator(),
                new HomeOwnerInsuranceExpenseEstimator(_appOptions),
                new CapExExpenseEstimator(_appOptions),
                new RepairExpenseEstimator(_appOptions),
                new PropertyManagementExpenseEstimator(_appOptions),
                new MiscExpenseEstimator(_appOptions));

            _mockExpenseService = new Mock<IExpenseService>();

            _expenseController = new ExpenseController(_expenseEstimator,
              _mockExpenseService.Object);
        }

        [Fact]
        public void EstimateExpenses()
        {
            var estimateExpenseRequest = new EstimateExpensesRequest();
            var expenseDetail = _expenseController.EstimateExpenses(estimateExpenseRequest);
            Assert.NotNull(expenseDetail);
        }

        [Fact]
        public void EstimateExpenses_GivenPropertyAddress()
        {
            // arrange 
            var testAddress = "1234 Heaven St, Anaheim, CA";
            var testHTML = "<html></html>";
            var interestRate = 7.0;


            var testListingDetail = new ListingDetail()
            {
                ListingPrice = 600000,
                YearBuilt = 1950,
                HoaFee = 200
            };

            var expectedExpenseDetail = new Dictionary<string, double>()
            {
                {nameof(CommonExpenseType.Misc), new Random().NextDouble() }
            };
            _mockExpenseService.Setup(expenseService => expenseService.CalculateExpenses(It.Is<string>(addr => addr == testAddress))).ReturnsAsync(expectedExpenseDetail);
            // act 
            var expenseDetail = _expenseController.EstimateExpenses(testAddress).Result;
            Assert.NotNull(expenseDetail);
            Assert.Equal(expectedExpenseDetail, expenseDetail);

        }

        [Fact]
        public void EstimateExpenses_ThrowExceptionOnNullArgument()
        {
            // arrange 
            EstimateExpensesRequest? estimateExpenseRequest = null;
            // assert
#pragma warning disable CS8604 // Possible null reference argument.
            var exception = Assert.Throws<ArgumentNullException>(() => _expenseController.EstimateExpenses(estimateExpenseRequest));
#pragma warning restore CS8604 // Possible null reference argument.
        }
    }
}
