using Core.Analyzer;
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
    public class MultiplePropertyAnalyzerTest
    {
        private readonly IPropertiesAnalyzer _propertiesAnalyzer;
        private readonly Mock<IZillowClient> _mockZillowClient;
        private readonly Mock<IHouseSearchParser> _mockHouseSearchParser;
        private readonly Mock<IMortgageInterestEstimator> _mockMortgageInterestEstimator;
        private readonly Mock<IExpenseEstimator> _mockExpenseEstimator;
        private readonly Mock<ITotalInvestmentEstimator> _mockOutOfPocketCostEstimator;
        private readonly Mock<IListingService> _mockListingService;
        private readonly Mock<IIncomesEstimator> _mockIncomeEstimator;
        private readonly AppOptions _appOptions;

        public MultiplePropertyAnalyzerTest()
        {
            _appOptions = new AppOptions()
            {
                DefaultDownPaymentPercent = 25,
                DefaultClosingCostOnBuy = 15000
            };

            _mockMortgageInterestEstimator = new Mock<IMortgageInterestEstimator>();
            _mockExpenseEstimator = new Mock<IExpenseEstimator>();
            _mockZillowClient = new Mock<IZillowClient>();
            _mockHouseSearchParser = new Mock<IHouseSearchParser>();
            _mockOutOfPocketCostEstimator = new Mock<ITotalInvestmentEstimator>();
            _mockListingService = new Mock<IListingService>();
            _ = new Mock<IPriceRentalService>();
            _mockIncomeEstimator = new Mock<IIncomesEstimator>();
            _propertiesAnalyzer = new PropertiesAnalyzer(
                _mockZillowClient.Object,
                _mockHouseSearchParser.Object,
                _mockMortgageInterestEstimator.Object,
                _mockOutOfPocketCostEstimator.Object,
                _mockExpenseEstimator.Object,
                _mockIncomeEstimator.Object,
                _mockListingService.Object,
                _appOptions);

        }

        [Fact]
        public void AnalyzeProperty()
        {
            // arrange. 
            var address = "123 Heaven St, Happiness City, Awesome State";
            var mockHtml = "<html></html>";
            _mockZillowClient.Setup(mockZillowClient => mockZillowClient.GetPriceMyRentalHtmlPage(It.Is<string>(value => value == address))).ReturnsAsync(mockHtml);

            const double ExpectedRentalIncome = 3000;

            _mockIncomeEstimator.Setup(incomesEstimator => incomesEstimator.EstimateIncomes(It.Is<string>(value => value == address))).ReturnsAsync(new Dictionary<string, double>()
            {
                { nameof(CommonIncomeType.Rental), ExpectedRentalIncome }
            });

            _mockZillowClient.Setup(mockZillowClient => mockZillowClient.GetListingHtmlPage(It.Is<string>(value => value == address))).ReturnsAsync(mockHtml);
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

            _mockExpenseEstimator.Setup(expenseEstimator => expenseEstimator.EstimateExpenses(It.IsAny<EstimateExpensesRequest>())).Returns(expenses);

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
