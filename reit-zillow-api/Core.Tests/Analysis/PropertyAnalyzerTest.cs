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
using System.Net.Sockets;

namespace Core.Tests.Analysis
{
    public class PropertyAnalyzerTest
    {
        private Mock<IZillowClient> _mockZillowClient;
        private Mock<IPriceRentalParser> _mockPriceRentalParser;
        private Mock<IListingParser> _mockListingParser;
        private Mock<IMortgageInterestEstimator> _mockMortgageInterestEstimator;
        private Mock<IExpenseEstimator> _mockExpenseEstimator;
        private readonly ITotalInvestmentEstimator _outOfPocketCostEstimator;
        private readonly IPropertyAnalyzer _propertyAnalyzer;
        private readonly AppOptions _appOptions;

        public PropertyAnalyzerTest()
        {
            _mockZillowClient = new Mock<IZillowClient>();
            _mockPriceRentalParser = new Mock<IPriceRentalParser>();
            _mockListingParser = new Mock<IListingParser>();
            _mockMortgageInterestEstimator = new Mock<IMortgageInterestEstimator>();
            _mockExpenseEstimator = new Mock<IExpenseEstimator>();

            _outOfPocketCostEstimator = new TotalInvestmentEstimator();

            _appOptions = new AppOptions()
            {
                DefaultDownPaymentPercent = 25,
                DefaultClosingCostOnBuy = 15000
            };

            _propertyAnalyzer = new PropertyAnalyzer(_mockPriceRentalParser.Object, _mockZillowClient.Object, _mockListingParser.Object, _mockMortgageInterestEstimator.Object,
              _mockExpenseEstimator.Object,
              _outOfPocketCostEstimator,
              _appOptions);
        }

        [Fact]
        public void AnalyzeProperty()
        {
            // arrange. 
            var address = "123 Heaven St, Happiness City, Awesome State";
            var mockHtml = "<html></html>";
            _mockZillowClient.Setup(mockZillowClient => mockZillowClient.GetPriceMyRentalHtmlPage(It.Is<string>(value => value == address))).ReturnsAsync(mockHtml);

            var mockPriceRentalDetail = new PriceRentalDetail()
            {
                ZEstimate = 3000,
                ZEstimateLow = 2000,
                ZEstimateHigh = 4000
            };
            _mockPriceRentalParser.Setup(parser => parser.Parse(It.Is<string>(value => value == mockHtml))).Returns(mockPriceRentalDetail);

            _mockZillowClient.Setup(mockZillowClient => mockZillowClient.GetListingHtmlPage(It.Is<string>(value => value == address))).ReturnsAsync(mockHtml);
            var listingDetail = new ListingDetail();
            listingDetail.ListingPrice = 123456;
            _mockListingParser.Setup(parser => parser.Parse(It.Is<string>(value => value == mockHtml))).Returns(listingDetail);

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
            var propertyAnalysisDetail = _propertyAnalyzer.AnalyzeProperty(address).Result;
            // assert 
            Assert.NotNull(propertyAnalysisDetail);
            Assert.NotNull(propertyAnalysisDetail!.Incomes);

            var expectedTotalIncome = mockPriceRentalDetail.ZEstimate;
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
