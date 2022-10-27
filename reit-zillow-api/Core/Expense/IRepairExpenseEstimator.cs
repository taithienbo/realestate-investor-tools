

namespace Core.Expense
{
    public interface IRepairExpenseEstimator
    {
        public double EstimateMonthlyAmount(int propertyAge);
    }
}
