namespace Core.Interest
{
    public interface IMortgageInterestService
    {
        public Task<double> GetCurrentInterest(double loanAmount, double propertyPrice);

        public Task<double> GetCurrentInterest(double propertyPrice);
    }
}
