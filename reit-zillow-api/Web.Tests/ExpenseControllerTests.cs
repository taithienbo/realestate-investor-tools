using Core.Constants;
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
        private readonly Mock<IZillowClient> _mockZillowClient;
        private readonly Mock<IZillowListingParser> _mockListingParser;
        private readonly Mock<IPriceRentalParser> _mockPriceRentalParser;
        private readonly Mock<IMortgageInterestEstimator> _mockMortgageInterestEstimator;
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

            _mockZillowClient = new Mock<IZillowClient>();
            _mockListingParser = new Mock<IZillowListingParser>();
            _mockPriceRentalParser = new Mock<IPriceRentalParser>();
            _mockMortgageInterestEstimator = new Mock<IMortgageInterestEstimator>();

            _expenseController = new ExpenseController(_expenseEstimator, _mockZillowClient.Object, _mockListingParser.Object, _mockPriceRentalParser.Object,
              _mockMortgageInterestEstimator.Object);
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
            _mockMortgageInterestEstimator.Setup(mortageInterestEstimator => mortageInterestEstimator.GetCurrentInterest(It.IsAny<double>(), It.IsAny<double>())).ReturnsAsync(interestRate);
            _mockZillowClient.Setup(zillowClient => zillowClient.GetListingHtmlPage(It.IsAny<string>())).ReturnsAsync(testHTML);
            var testListingDetail = new ListingDetail()
            {
                ListingPrice = 600000,
                YearBuilt = 1950,
                HoaFee = 200
            };
            var testPriceRentalDetail = new PriceRentalDetail()
            {
                ZEstimateLow = 1,
                ZEstimateHigh = 10,
                ZEstimate = 5
            };
            _mockListingParser.Setup(listingParser => listingParser.Parse(It.IsAny<string>())).Returns(testListingDetail);
            _mockPriceRentalParser.Setup(priceRentalParser => priceRentalParser.Parse(It.IsAny<string>())).Returns(testPriceRentalDetail);
            // act 
            var expenseDetail = _expenseController.EstimateExpenses(testAddress).Result;
            Assert.NotNull(expenseDetail);
            foreach (var commonExpense in Enum.GetNames(typeof(CommonExpenseType)))
            {
                Assert.True(expenseDetail.ContainsKey(commonExpense));
            }

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
