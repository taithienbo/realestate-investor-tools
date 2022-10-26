

namespace Core.ExpenseEstimator
{
    public interface IRepairExpenseEstimator
    {
        public double EstimateMonthlyAmount(int propertyAge);
    }
}
