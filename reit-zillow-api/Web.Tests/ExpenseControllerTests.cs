using Core.Constants;
using Core.Dto;
using Core.Expense;
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
            _expenseController = new ExpenseController(_expenseEstimator, _mockZillowClient.Object, _mockListingParser.Object);
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
            _mockZillowClient.Setup(zillowClient => zillowClient.GetHtml(It.IsAny<string>())).ReturnsAsync(testHTML);
            var testListingDetail = new ListingDetail()
            {
                ListingPrice = 600000,
                YearBuilt = 1950,
            };
            _mockListingParser.Setup(listingParser => listingParser.Parse(It.IsAny<string>())).Returns(testListingDetail);
            // act 
            var expenseDetail = _expenseController.EstimateExpenses(testAddress).Result;
            Assert.NotNull(expenseDetail);
            Assert.True(expenseDetail.Total > 0);

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
