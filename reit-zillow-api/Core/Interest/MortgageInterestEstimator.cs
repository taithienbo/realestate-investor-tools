
using Core.ConsumerFinance;


namespace Core.Interest
{
    public class MortgageInterestEstimator : IMortgageInterestEstimator
    {
        private readonly IRateCheckerApiClient _rateCheckerApiClient;
        private const double DefaultDownPaymentPercent = 25;

        public MortgageInterestEstimator(IRateCheckerApiClient rateCheckerApiClient)
        {
            _rateCheckerApiClient = rateCheckerApiClient;
        }

        public async Task<double> GetCurrentInterest(double loanAmount, double propertyPrice)
        {
            var rateCheckerResponse = await _rateCheckerApiClient.CheckRate(new RateCheckerRequestInfo()
            {
                LoanAmount = loanAmount,
                Price = propertyPrice
            });
            if (rateCheckerResponse == null || rateCheckerResponse.Data == null)
            {
                return 0;
            }
            // use the interest rate which has the highest number of lenders
            return rateCheckerResponse.Data.MaxBy(keyValuePair => keyValuePair.Value).Key;
        }

        public Task<double> GetCurrentInterest(double propertyPrice)
        {
            double loanAmount = Calculators.CalculateLoanAmount(propertyPrice, DefaultDownPaymentPercent);
            return GetCurrentInterest(loanAmount, propertyPrice);
        }


    }
}
