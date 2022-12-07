using Core.Options;

namespace Core.Expense
{
    public class RepairExpenseEstimator : IRepairExpenseEstimator
    {

        private readonly AppOptions _appOptions;

        public RepairExpenseEstimator(AppOptions appOptions)
        {
            _appOptions = appOptions;
        }

        public double EstimateMonthlyAmount(int propertyAge)
        {
            return _appOptions.BaseRepairMonthlyAmount + propertyAge;
        }
    }
}
