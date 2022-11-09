﻿using Core.ConsumerFinance;
using Core.Dto;
using Core.Interest;
using Infrastructure.Interest;
using Moq;

namespace Infrastructure.Tests.Interest
{
    public class MortgageInterestEstimatorTests
    {
        private IMortgageInterestEstimator _mortgageInterestEstimator;
        private Mock<IRateCheckerApiClient> _mockRateCheckerApiClient;

        public MortgageInterestEstimatorTests()
        {
            _mockRateCheckerApiClient = new Mock<IRateCheckerApiClient>();
            _mortgageInterestEstimator = new MortgageInterestEstimator(_mockRateCheckerApiClient.Object);
        }

        [Fact]
        public void GetCurrentInterest_UseTheInterestValueWithHighestNumOfLendersOffer()
        {
            // arrange 
            var propertyZipCode = 12345;
            var loanAmount = 123456;
            var rateData = new Dictionary<double, int>(); rateData[7.1] = 2; rateData[5.2] = 4;
            var rateWithHighestNumOfLenders = rateData.MaxBy(keyValuePair => keyValuePair.Value).Key;
            var rateCheckerRespone = new RateCheckerResponse()
            {
                Data = rateData,
                Request = new RateCheckerRequestInfo()
            };
            _mockRateCheckerApiClient.Setup(rateCheckerClient => rateCheckerClient.CheckRate(It.IsAny<RateCheckerRequestInfo>())).ReturnsAsync(rateCheckerRespone);
            // act 
            var interestRate = _mortgageInterestEstimator.GetCurrentInterest(propertyZipCode, loanAmount).Result;
            Assert.Equal(rateWithHighestNumOfLenders, interestRate, 0);
        }
    }
}
