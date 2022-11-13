namespace Core.Interest
{
    public interface IMortgageInterestEstimator
    {
        public Task<double> GetCurrentInterest(double loanAmount, double propertyPrice);

        public Task<double> GetCurrentInterest(double propertyPrice);
    }
}
