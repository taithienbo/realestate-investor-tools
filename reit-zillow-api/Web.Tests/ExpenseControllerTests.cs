using Core.Constants;
using Core.Dto;
using Core.Expense;
using Core.Income;
using Core.Interest;
using Core.Listing;
using Core.Zillow;
using Infrastructure.Expense;
using Infrastructure.Listing;
using Moq;
using reit_zillow_api.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace Web.Tests
{
    public class ExpenseControllerTests
    {
        private readonly ExpenseController _expenseController;
        private readonly IExpenseEstimator _expenseEstimator;
        private readonly Mock<IZillowClient> _mockZillowClient;
        private readonly Mock<IListingParser> _mockListingParser;
        private readonly Mock<IPriceRentalParser> _mockPriceRentalParser;
        private readonly Mock<IMortgageInterestEstimator> _mockMortgageInterestEstimator;

        public ExpenseControllerTests()
        {
            // need to update to real expense estimator 
            _expenseEstimator = new ExpenseEstimator(new MortgageExpenseEstimator(),
                new PropertyTaxExpenseEstimator(),
                new HomeOwnerInsuranceExpenseEstimator(),
                new CapExExpenseEstimator(),
                new RepairExpenseEstimator(),
                new PropertyManagementExpenseEstimator(),
                new MiscExpenseEstimator());
            _mockZillowClient = new Mock<IZillowClient>();
            _mockListingParser = new Mock<IListingParser>();
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
