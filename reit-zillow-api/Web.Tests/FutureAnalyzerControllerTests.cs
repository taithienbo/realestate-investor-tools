using Core.Analyzer;
using Core.Dto;
using Core.Interest;
using Core.Listing;
using Core.Loan;
using Core.Options;
using Microsoft.AspNetCore.Mvc.Routing;
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
    public class FutureAnalyzerControllerTests
    {
        private readonly FuturePropertyAnalyzerController _controller;
        private readonly Mock<IFutureAnalyzer> _futureAnalyzerMock;
        private readonly Mock<IMortgageInterestEstimator> _mockMortgageInterestEstimator;
        private readonly Mock<IZillowListingService> _mockZillowListingService;
        private readonly AppOptions _appOptions;

        public FutureAnalyzerControllerTests()
        {
            _futureAnalyzerMock = new Mock<IFutureAnalyzer>();
            _mockMortgageInterestEstimator = new Mock<IMortgageInterestEstimator>();
            _mockZillowListingService = new Mock<IZillowListingService>();

            _appOptions = new AppOptions()
            {
                DefaultDownPaymentPercent = 25
            };

            _controller = new FuturePropertyAnalyzerController(_futureAnalyzerMock.Object, _mockZillowListingService.Object, _mockMortgageInterestEstimator.Object,
                _appOptions);
        }

        [Fact]
        public void AnalyzeByRequestForm()
        {
            // arrange 
            var analyzeMethodParams = new FutureAnalyzerRequest()
            {
                DownPaymentAmount = 60000,
                OriginalLoanAmount = 432200,
                HoldingPeriodInYears = 5,
                InterestRate = 2.75,
                LoanProgram = LoanProgram.ThirtyYearFixed
            };

            const double ExpectedAmount = 180259.72;
            const double ExpectedAmountPerMonth = 3004.328;
            _futureAnalyzerMock.Setup(analyzer => analyzer.CalculateNetProfitsOnSell(It.Is<FutureAnalyzerRequest>(parameters => parameters == analyzeMethodParams))).Returns(ExpectedAmount);

            // act 
            FutureAnalyzerResponse futureAnalyzerResponse = _controller.AnalyzeInvestmentAfterHoldingPeriod(analyzeMethodParams);
            Assert.NotNull(futureAnalyzerResponse);
            Assert.Equal(ExpectedAmount, futureAnalyzerResponse.TotalMoneyMadeAfterHold, 0);
            Assert.Equal(ExpectedAmountPerMonth, futureAnalyzerResponse.MoneyMadePerMonth, 0);
            Assert.NotNull(futureAnalyzerResponse.Inputs);
        }

        [Fact]
        public void AnalyzeByAddress()
        {
            // arrange. 
            const string Address = "1234 Test Ave, Awesome State, CA";
            const int NumOfYearsHold = 5;

            const double InterestRate = 7.0;
            const double ListingPrice = 700000;
            const double DownPaymentAmount = 175000; // 25% of 700000 
            const double OriginalLoanAmount = 525000;   // 700000 - 175000
            var listingDetail = new ListingDetail()
            {
                ListingPrice = ListingPrice
            };

            _mockZillowListingService.Setup(zillowListingService => zillowListingService.GetListingDetail(It.Is<string>(addr => addr == Address))).ReturnsAsync(listingDetail);


            _mockMortgageInterestEstimator.Setup(interestEstimator => interestEstimator.GetCurrentInterest(It.Is<double>(price => price == ListingPrice))).ReturnsAsync(InterestRate);

            _mockMortgageInterestEstimator.Setup(interestEstimator => interestEstimator.GetCurrentInterest(It.IsAny<double>(), It.IsAny<double>())).ReturnsAsync(InterestRate);

            var expectedFutureAnalyzerRequest = new FutureAnalyzerRequest()
            {
                DownPaymentAmount = DownPaymentAmount,
                OriginalLoanAmount = OriginalLoanAmount,
                HoldingPeriodInYears = NumOfYearsHold,
                InterestRate = InterestRate
            };

            const double ExpectedTotalMoneyMadeAfterHold = 200000;

            _futureAnalyzerMock.Setup(futureAnalyzer => futureAnalyzer.CalculateNetProfitsOnSell(It.IsAny<FutureAnalyzerRequest>())).Returns(ExpectedTotalMoneyMadeAfterHold);

            // act 
            FutureAnalyzerResponse futureAnalyzerResponse = _controller.AnalyzeInvestmentAfterHoldingPeriod(Address, NumOfYearsHold).Result;
            // assert 
            Assert.NotNull(futureAnalyzerResponse);
            Assert.NotNull(futureAnalyzerResponse.Inputs);
            Assert.Equal(expectedFutureAnalyzerRequest.DownPaymentAmount, futureAnalyzerResponse?.Inputs?.DownPaymentAmount);
            Assert.Equal(expectedFutureAnalyzerRequest.OriginalLoanAmount, futureAnalyzerResponse?.Inputs?.OriginalLoanAmount);
            Assert.Equal(expectedFutureAnalyzerRequest.HoldingPeriodInYears, futureAnalyzerResponse?.Inputs?.HoldingPeriodInYears);
            Assert.Equal(expectedFutureAnalyzerRequest.InterestRate, futureAnalyzerResponse?.Inputs?.InterestRate);
            Assert.Equal(ExpectedTotalMoneyMadeAfterHold, futureAnalyzerResponse?.TotalMoneyMadeAfterHold);
            Assert.True(futureAnalyzerResponse?.MoneyMadePerMonth > 0);
        }
    }
}
