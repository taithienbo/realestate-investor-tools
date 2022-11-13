namespace Core.Expense
{
    public class HomeOwnerInsuranceExpenseEstimator
        : IHomeOwnerInsuranceExpenseEstimator
    {
        private const double EstimateHomeOwnerInsurancePercentageOfListingPrice = .25;

        public double CalculateMonthlyAmount(double listingPrice)
        {
            return listingPrice * EstimateHomeOwnerInsurancePercentageOfListingPrice / 100 / 12;
        }
    }
}
