using Core.ConsumerFinance;
using Core.Interest;
using Core.Options;
using Moq;

namespace Infrastructure.Tests.Interest
{
    public class MortgageInterestEstimatorTests
    {
        private IMortgageInterestEstimator _mortgageInterestEstimator;
        private Mock<IRateCheckerApiClient> _mockRateCheckerApiClient;
        private readonly AppOptions _appOptions;

        public MortgageInterestEstimatorTests()
        {
            _appOptions = new AppOptions()
            {
                DefaultDownPaymentPercent = 25
            };
            _mockRateCheckerApiClient = new Mock<IRateCheckerApiClient>();
            _mortgageInterestEstimator = new MortgageInterestEstimator(_mockRateCheckerApiClient.Object, _appOptions);
        }

        [Fact]
        public void GetCurrentInterest_UseTheInterestValueWithHighestNumOfLendersOffer()
        {
            // arrange 
            var loanAmount = 123456;
            var propertyPrice = 456789;
            var rateData = new Dictionary<double, int>(); rateData[7.1] = 2; rateData[5.2] = 4;
            var rateWithHighestNumOfLenders = rateData.MaxBy(keyValuePair => keyValuePair.Value).Key;
            var rateCheckerRespone = new RateCheckerResponse()
            {
                Data = rateData,
                Request = new RateCheckerRequestInfo()
            };
            _mockRateCheckerApiClient.Setup(rateCheckerClient => rateCheckerClient.CheckRate(It.IsAny<RateCheckerRequestInfo>())).ReturnsAsync(rateCheckerRespone);
            // act 
            var interestRate = _mortgageInterestEstimator.GetCurrentInterest(loanAmount, propertyPrice).Result;
            Assert.Equal(rateWithHighestNumOfLenders, interestRate, 0);
        }

        [Fact]
        public void GetCurrentInterestRate_UseDefaultLoanAmountGivenPropertyPrice()
        {
            // arrange 
            var propertyPrice = 456789;
            var rateData = new Dictionary<double, int>();
            rateData[7.1] = 2;
            var rateCheckerRespone = new RateCheckerResponse()
            {
                Data = rateData,
                Request = new RateCheckerRequestInfo()
            };
            _mockRateCheckerApiClient.Setup(rateCheckerClient => rateCheckerClient.CheckRate(It.IsAny<RateCheckerRequestInfo>())).ReturnsAsync(rateCheckerRespone);
            // act
            var interestRate = _mortgageInterestEstimator.GetCurrentInterest(propertyPrice).Result;
            Assert.Equal(rateData.First().Key, interestRate, 0);
        }
    }
}
