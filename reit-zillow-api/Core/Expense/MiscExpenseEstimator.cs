using Core.Options;

namespace Core.Expense
{
    public class MiscExpenseEstimator : IMiscExpenseEstimator
    {
        private readonly AppOptions _appOptions;

        public MiscExpenseEstimator(AppOptions appOptions)
        {
            _appOptions = appOptions;
        }

        public double EstimateMonthlyAmount()
        {
            return _appOptions.BaseMiscExpenseMonthlyAmount;
        }
    }
}
