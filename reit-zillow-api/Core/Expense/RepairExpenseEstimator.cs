namespace Core.Expense
{
    public class RepairExpenseEstimator : IRepairExpenseEstimator
    {
        private const double BaseMonthlyAmount = 110;

        public double EstimateMonthlyAmount(int propertyAge)
        {
            return BaseMonthlyAmount + propertyAge;
        }
    }
}
