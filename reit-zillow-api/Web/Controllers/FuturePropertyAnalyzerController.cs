﻿using Core;
using Core.Analyzer;
using Core.Dto;
using Core.Interest;
using Core.Listing;
using Core.Loan;
using Core.Options;
using Core.Zillow;
using Microsoft.AspNetCore.Mvc;

namespace reit_zillow_api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class FuturePropertyAnalyzerController : ControllerBase
    {
        private readonly IFutureAnalyzer _futureAnalyzer;
        private readonly IZillowListingService _zillowListingService;
        private readonly IMortgageInterestEstimator _mortgageInterestEstimator;
        private readonly AppOptions _appOptions;

        public FuturePropertyAnalyzerController(IFutureAnalyzer futureAnalyzer,
            IZillowListingService zillowListingService,
            IMortgageInterestEstimator interestEstimator,
            AppOptions appOptions)
        {
            _futureAnalyzer = futureAnalyzer;
            _zillowListingService = zillowListingService;
            _mortgageInterestEstimator = interestEstimator;
            _appOptions = appOptions;
        }

        [HttpGet]
        public FutureAnalyzerResponse AnalyzeInvestmentAfterHoldingPeriod([FromQuery] FutureAnalyzerRequest investmentOnSellAnalyzerParams)
        {
            double moneyMadeOrLoseAfterHold = _futureAnalyzer.CalculateNetProfitsOnSell(investmentOnSellAnalyzerParams);
            return new FutureAnalyzerResponse()
            {
                TotalMoneyMadeAfterHold = moneyMadeOrLoseAfterHold,
                Inputs = investmentOnSellAnalyzerParams
            };
        }

        [HttpGet]
        public async Task<FutureAnalyzerResponse> AnalyzeInvestmentAfterHoldingPeriod(string address, int numOfYearsHold)
        {
            var listingDetail = await _zillowListingService.GetListingDetail(address);

            double loanAmount =
            Calculators.CalculateLoanAmount(listingDetail.ListingPrice, _appOptions.DefaultDownPaymentPercent);
            double interestRate = await _mortgageInterestEstimator.GetCurrentInterest(loanAmount, listingDetail.ListingPrice);

            var inputs = new FutureAnalyzerRequest()
            {
                DownPaymentAmount = Calculators.CalculateDownPayment(listingDetail.ListingPrice, _appOptions.DefaultDownPaymentPercent),
                OriginalLoanAmount = loanAmount,
                OriginalPurchaseAmount = listingDetail.ListingPrice,
                HoldingPeriodInYears = numOfYearsHold,
                InterestRate = interestRate,
                LoanProgram = LoanProgram.ThirtyYearFixed
            };

            return AnalyzeInvestmentAfterHoldingPeriod(inputs);
        }
    }
}
