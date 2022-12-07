using Core.Options;

namespace Core.Expense
{
    public class HomeOwnerInsuranceExpenseEstimator
        : IHomeOwnerInsuranceExpenseEstimator
    {

        private readonly AppOptions _appOptions;

        public HomeOwnerInsuranceExpenseEstimator(AppOptions appOptions)
        {
            _appOptions = appOptions;
        }

        public double CalculateMonthlyAmount(double listingPrice)
        {
            return listingPrice * _appOptions.BaseHomeOwnerInsurancePercentageOfPropertyValue / 100 / 12;
        }
    }
}
