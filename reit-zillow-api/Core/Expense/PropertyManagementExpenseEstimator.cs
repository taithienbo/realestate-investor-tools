using Core.Options;

namespace Core.Expense
{
    public class PropertyManagementExpenseEstimator : IPropertyManagementExpenseEstimator
    {
        private readonly AppOptions _appOptions;

        public PropertyManagementExpenseEstimator(AppOptions appOptions)
        {
            _appOptions = appOptions;
        }

        public double EstimateMonthlyAmount(double rentAmount)
        {
            return _appOptions.BasePropertyManagementCostAsPercentageOfMonthlyRent / 100 * rentAmount;
        }
    }
}
