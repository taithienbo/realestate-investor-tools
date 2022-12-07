using Core.Options;

namespace Core.Expense
{
    public class CapExExpenseEstimator : ICapExExpenseEstimator
    {

        private readonly AppOptions _appOptions;

        public CapExExpenseEstimator(AppOptions appOptions)
        {
            _appOptions = appOptions;
        }

        public double CalculateEstimatedMonthlyCapEx(double propertyValue, int propertyAge)
        {
            var monthlyBaseAmount = _appOptions.BaseCapExPercentOfPropertyValue / 100 * propertyValue / 12;
            double monthlyAdditionalAmountBasedOnAge = propertyAge;
            var estimatedMonthlyAmount = monthlyBaseAmount + monthlyAdditionalAmountBasedOnAge;
            return estimatedMonthlyAmount;
        }
    }
}
