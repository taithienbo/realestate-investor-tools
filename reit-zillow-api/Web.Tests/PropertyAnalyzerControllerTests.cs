﻿using Core.Constants;
using Core.Dto;
using Core.Expense;
using Core.Income;
using Core.Interest;
using Core.Listing;
using Core.Zillow;
using Moq;
using reit_zillow_api.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Tests
{
    public class PropertyAnalyzerControllerTests
    {
        private readonly PropertyAnalyzerController _propertyAnalyzerController;

        private Mock<IZillowClient> _mockZillowClient;
        private Mock<IPriceRentalParser> _mockPriceRentalParser;
        private Mock<IListingParser> _mockListingParser;
        private Mock<IMortgageInterestEstimator> _mockMortgageInterestEstimator;
        private Mock<IExpenseEstimator> _mockExpenseEstimator;

        public PropertyAnalyzerControllerTests()
        {
            _mockZillowClient = new Mock<IZillowClient>();
            _mockPriceRentalParser = new Mock<IPriceRentalParser>();
            _mockListingParser = new Mock<IListingParser>();
            _mockMortgageInterestEstimator = new Mock<IMortgageInterestEstimator>();
            _mockExpenseEstimator = new Mock<IExpenseEstimator>();

            _propertyAnalyzerController = new PropertyAnalyzerController(_mockPriceRentalParser.Object, _mockZillowClient.Object, _mockListingParser.Object, _mockMortgageInterestEstimator.Object,
              _mockExpenseEstimator.Object);
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
            };

            _mockExpenseEstimator.Setup(expenseEstimator => expenseEstimator.EstimateExpenses(It.IsAny<EstimateExpensesRequest>())).Returns(expenses);

            // act 
            var propertyAnalysisDetail = _propertyAnalyzerController.Analyze(address).Result;
            // assert 
            Assert.NotNull(propertyAnalysisDetail);
            Assert.NotNull(propertyAnalysisDetail!.Incomes);
            var expectedTotalIncome = mockPriceRentalDetail.ZEstimate; 
            Assert.True(propertyAnalysisDetail.Incomes!.ContainsKey(nameof(CommonIncomeType.Rental)));
            Assert.Equal(expectedTotalIncome, propertyAnalysisDetail.TotalIncome));
            Assert.NotNull(propertyAnalysisDetail.ListingDetail);
            Assert.Equal(interestRate, propertyAnalysisDetail.InterestRate, 0);
            Assert.NotNull(propertyAnalysisDetail.Expenses);
            var expectedTotalExpense = expenses.Sum(keyVaulue => keyVaulue.Value);  
            foreach (var commonExpense in Enum.GetNames(typeof(CommonExpenseType)))
            {
                Assert.True(propertyAnalysisDetail.Expenses!.ContainsKey(commonExpense));
            }
            Assert.Equal(expectedTotalExpense, propertyAnalysisDetail.TotalExpense);          
        }
    }
}