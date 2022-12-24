using Core.Analyzer;
using Core.CashFlow;
using Core.Dto;
using Core.Income;
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
        private readonly Mock<IListingService> _mockZillowListingService;
        private readonly Mock<ICashFlowCalculator> _mockCashFlowCalculator;
        private readonly AppOptions _appOptions;

        public FutureAnalyzerControllerTests()
        {
            _futureAnalyzerMock = new Mock<IFutureAnalyzer>();
            _mockMortgageInterestEstimator = new Mock<IMortgageInterestEstimator>();
            _mockZillowListingService = new Mock<IListingService>();
            _mockCashFlowCalculator = new Mock<ICashFlowCalculator>();

            _appOptions = new AppOptions()
            {
                DefaultDownPaymentPercent = 25
            };

            _controller = new FuturePropertyAnalyzerController(_futureAnalyzerMock.Object,
                _mockZillowListingService.Object, _mockMortgageInterestEstimator.Object,
                _mockCashFlowCalculator.Object,
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
                LoanProgram = LoanProgram.ThirtyYearFixed,
                EstimatedMonthlyCashflow = 100
            };

            double ExpectedTotalCashflow = analyzeMethodParams.EstimatedMonthlyCashflow * analyzeMethodParams.HoldingPeriodInYears * 12;
            double expectedMonthlyCashFlow = ExpectedTotalCashflow / (analyzeMethodParams.HoldingPeriodInYears * 12);

            const double ExpectedAmountAfterHoldWithoutCashFlow = 180259.72;
            const double ExpectedAmountPerMonthAfterHoldWithoutCashFlow = 3004.328;
            double expectedAmountAfterHoldWithCashFlow = ExpectedTotalCashflow + ExpectedAmountAfterHoldWithoutCashFlow;
            double expectedAmountPerMonthAfterHoldWithCashFlow = ExpectedAmountPerMonthAfterHoldWithoutCashFlow + expectedMonthlyCashFlow;

            _futureAnalyzerMock.Setup(analyzer => analyzer.CalculateNetProfitsOnSell(It.Is<FutureAnalyzerRequest>(parameters => parameters == analyzeMethodParams))).Returns(ExpectedAmountAfterHoldWithoutCashFlow);

            // act 
            FutureAnalyzerResponse futureAnalyzerResponse = _controller.AnalyzeInvestmentAfterHoldingPeriod(analyzeMethodParams);
            // assert 
            Assert.NotNull(futureAnalyzerResponse);
            Assert.Equal(ExpectedAmountAfterHoldWithoutCashFlow, futureAnalyzerResponse.TotalAmountAfterHoldWithoutCashFlow, 0);
            Assert.Equal(ExpectedAmountPerMonthAfterHoldWithoutCashFlow, futureAnalyzerResponse.AmountPerMonthWithoutCashFlow, 0);
            Assert.NotNull(futureAnalyzerResponse.AnalyzerInputs);
            Assert.NotNull(futureAnalyzerResponse?.AnalyzerConfigs);
            Assert.Equal(analyzeMethodParams.EstimatedMonthlyCashflow, futureAnalyzerResponse?.EstimatedMonthlyCashflow);
            Assert.Equal(expectedAmountAfterHoldWithCashFlow, futureAnalyzerResponse?.TotalAmountAfterHoldWithCashFlow);
            Assert.Equal(expectedAmountPerMonthAfterHoldWithCashFlow,
                futureAnalyzerResponse!.AmountPerMonthWithCashFlow, 0);
        }

        [Fact]
        public void AnalyzeByAddress()
        {
            // arrange. 
            const string Address = "1234 Test Ave, Awesome State, CA";
            const int NumOfYearsHold = 5;

            const double ListingPrice = 700000;
            const double DownPaymentAmount = 175000; // 25% of 700000 
            const double OriginalLoanAmount = 525000;   // 700000 - 175000
            
            var listingDetail = new ListingDetail()
            {
                ListingPrice = ListingPrice
            };
            _mockZillowListingService.Setup(zillowListingService => zillowListingService.GetListingDetail(It.Is<string>(addr => addr == Address))).ReturnsAsync(listingDetail);

            const double InterestRate = 7.0;
            _mockMortgageInterestEstimator.Setup(interestEstimator => interestEstimator.GetCurrentInterest(It.IsAny<double>())).ReturnsAsync(InterestRate);

            _mockCashFlowCalculator.Setup(cashFlowCalculator => cashFlowCalculator.CalculateCashFlow(Address)).ReturnsAsync(new Random().NextDouble());

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
            Assert.NotNull(futureAnalyzerResponse.AnalyzerInputs);
            Assert.Equal(expectedFutureAnalyzerRequest.DownPaymentAmount, futureAnalyzerResponse?.AnalyzerInputs?.DownPaymentAmount);
            Assert.Equal(expectedFutureAnalyzerRequest.OriginalLoanAmount, futureAnalyzerResponse?.AnalyzerInputs?.OriginalLoanAmount);
            Assert.Equal(expectedFutureAnalyzerRequest.HoldingPeriodInYears, futureAnalyzerResponse?.AnalyzerInputs?.HoldingPeriodInYears);
            Assert.Equal(expectedFutureAnalyzerRequest.InterestRate, futureAnalyzerResponse?.AnalyzerInputs?.InterestRate);
            Assert.Equal(ExpectedTotalMoneyMadeAfterHold, futureAnalyzerResponse?.TotalAmountAfterHoldWithoutCashFlow);
            Assert.True(futureAnalyzerResponse?.AmountPerMonthWithoutCashFlow > 0);
            Assert.True(futureAnalyzerResponse!.EstimatedMonthlyCashflow > 0);
            Assert.NotNull(futureAnalyzerResponse?.AnalyzerConfigs);

        }
    }
}
