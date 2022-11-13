namespace Core.Expense
{
    public interface IPropertyManagementExpenseEstimator
    {
        public double EstimateMonthlyAmount(double rentAmount);
    }
}
