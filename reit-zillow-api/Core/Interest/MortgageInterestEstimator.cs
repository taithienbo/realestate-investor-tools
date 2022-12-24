
using Core.Constants;
using Core.ConsumerFinance;
using Core.Options;

namespace Core.Interest
{
    public class MortgageInterestEstimator : IMortgageInterestEstimator
    {
        private readonly IRateCheckerApiClient _rateCheckerApiClient;
        private readonly AppOptions _appOptions;
        public MortgageInterestEstimator(IRateCheckerApiClient rateCheckerApiClient, AppOptions appOptions)
        {
            _rateCheckerApiClient = rateCheckerApiClient;
            _appOptions = appOptions;
        }

        public async Task<double> GetCurrentInterest(double loanAmount, double propertyPrice)
        {
            var rateCheckerResponse = await _rateCheckerApiClient.CheckRate(new RateCheckerRequestInfo()
            {
                LoanAmount = loanAmount,
                Price = propertyPrice
            });
            if (rateCheckerResponse == null || rateCheckerResponse.Data == null || rateCheckerResponse.Data.Count == 0)
            {
                return 0;
            }
            // use the interest rate which has the highest number of lenders
            return rateCheckerResponse.Data.MaxBy(keyValuePair => keyValuePair.Value).Key;
        }

        public Task<double> GetCurrentInterest(double propertyPrice)
        {
            double loanAmount = Calculators.CalculateLoanAmount(propertyPrice, _appOptions.DefaultDownPaymentPercent);
            return GetCurrentInterest(loanAmount, propertyPrice);
        }
    }
}
