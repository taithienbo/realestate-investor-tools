﻿using Core.Analyzer;
using Core.Constants;
using Core.Dto;
using Core.Expense;
using Core.Income;
using Core.Interest;
using Core.Listing;
using Core.Options;
using Core.Zillow;
using Moq;

namespace Core.Tests.Analysis
{
    public class PropertiesAnalyzerTests
    {
        private readonly IPropertiesAnalyzer _propertiesAnalyzer;
        private readonly Mock<IMortgageInterestService> _mockMortgageInterestEstimator;
        private readonly Mock<IExpenseService> _mockExpenseService;
        private readonly Mock<ITotalInvestmentEstimator> _mockOutOfPocketCostEstimator;
        private readonly Mock<IListingService> _mockListingService;
        private readonly Mock<IIncomesService> _mockIncomeEstimator;
        private readonly Mock<IHouseSearchService> _mockHouseSearchService;

        private readonly AppOptions _appOptions;

        public PropertiesAnalyzerTests()
        {
            _appOptions = new AppOptions()
            {
                DefaultDownPaymentPercent = 25,
                DefaultClosingCostOnBuy = 15000
            };

            _mockMortgageInterestEstimator = new Mock<IMortgageInterestService>();
            _mockExpenseService = new Mock<IExpenseService>();
            _mockHouseSearchService = new Mock<IHouseSearchService>();
            _mockOutOfPocketCostEstimator = new Mock<ITotalInvestmentEstimator>();
            _mockListingService = new Mock<IListingService>();
            _ = new Mock<IPriceRentalService>();
            _mockIncomeEstimator = new Mock<IIncomesService>();
            _propertiesAnalyzer = new PropertiesAnalyzer(

                _mockMortgageInterestEstimator.Object,
                _mockOutOfPocketCostEstimator.Object,
                _mockExpenseService.Object,
                _mockIncomeEstimator.Object,
                _mockListingService.Object,
                _mockHouseSearchService.Object,
                _appOptions);
        }

        [Fact]
        public void AnalyzeProperty()
        {
            // arrange. 
            var address = "123 Heaven St, Happiness City, Awesome State";

            const double ExpectedRentalIncome = 3000;

            _mockIncomeEstimator.Setup(incomesEstimator => incomesEstimator.EstimateIncomes(It.Is<string>(value => value == address))).ReturnsAsync(new Dictionary<string, double>()
            {
                { nameof(CommonIncomeType.Rental), ExpectedRentalIncome }
            });

            var listingDetail = new ListingDetail();
            listingDetail.ListingPrice = 123456;

            _mockListingService.Setup(listingService => listingService.GetListingDetail(It.IsAny<string>())).ReturnsAsync(listingDetail);

            var interestRate = 7.0;
            _mockMortgageInterestEstimator.Setup(interestEstimator => interestEstimator.GetCurrentInterest(listingDetail.ListingPrice)).ReturnsAsync(interestRate);

            var expenses = new Dictionary<string, double>()
            {
                { nameof(CommonExpenseType.PropertyManagement), 100 },
                { nameof(CommonExpenseType.PropertyTax), 100 },
                { nameof(CommonExpenseType.CapitalExpenditures), 100 },
                { nameof(CommonExpenseType.HomeOwnerInsurance), 100 },
                { nameof(CommonExpenseType.Misc), 100 },
                { nameof(CommonExpenseType.Mortgage), 100 },
                { nameof(CommonExpenseType.Repairs), 100 },
                { nameof(CommonExpenseType.HoaFee), 200 }
    };

            _mockExpenseService.Setup(expenseService => expenseService.CalculateExpenses(It.Is<string>(addr => addr == address))).ReturnsAsync(expenses);

            // act 
            var propertyAnalysisDetail = _propertiesAnalyzer.Analyze(address).Result;
            // assert 
            Assert.NotNull(propertyAnalysisDetail);
            Assert.NotNull(propertyAnalysisDetail!.Incomes);

            var expectedTotalIncome = ExpectedRentalIncome;
            Assert.True(propertyAnalysisDetail.Incomes!.ContainsKey(nameof(CommonIncomeType.Rental)));
            Assert.Equal(expectedTotalIncome, propertyAnalysisDetail.TotalIncome);
            Assert.NotNull(propertyAnalysisDetail.ListingDetail);
            Assert.Equal(interestRate, propertyAnalysisDetail.InterestRate, 0);
            Assert.NotNull(propertyAnalysisDetail.Expenses);

            var expectedTotalExpense = expenses.Sum(keyVaulue => keyVaulue.Value);
            foreach (var commonExpense in Enum.GetNames(typeof(CommonExpenseType)))
            {
                Assert.True(propertyAnalysisDetail.Expenses!.ContainsKey(commonExpense));
            }
            Assert.Equal(expectedTotalExpense, propertyAnalysisDetail.TotalExpense);

            Assert.True(propertyAnalysisDetail.NetOperatingIncome != 0);
            Assert.True(propertyAnalysisDetail.CapRate != 0);
            Assert.True(propertyAnalysisDetail.DebtServiceCoverageRatio != 0);
            Assert.True(propertyAnalysisDetail.CashFlow != 0);
            Assert.True(propertyAnalysisDetail.CashOnCashReturn != 0);
            Assert.True(propertyAnalysisDetail.CashOnCashReturn != 0);

            Assert.Equal(_appOptions.DefaultDownPaymentPercent, propertyAnalysisDetail.AssumedDownPaymentPercent, 0);
            Assert.Equal(_appOptions.DefaultClosingCostOnBuy, propertyAnalysisDetail.AssumedClosingCost, 0);

            Assert.NotNull(propertyAnalysisDetail.AssumedOutOfPocketCosts);
        }
    }
}
