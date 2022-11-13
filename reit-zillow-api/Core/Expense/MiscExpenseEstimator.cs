namespace Core.Expense
{
    public class MiscExpenseEstimator : IMiscExpenseEstimator
    {
        private const double BaseMiscExpenseEstimatedMonthlyAmount = 100;

        public double EstimateMonthlyAmount()
        {
            return BaseMiscExpenseEstimatedMonthlyAmount;
        }
    }
}
