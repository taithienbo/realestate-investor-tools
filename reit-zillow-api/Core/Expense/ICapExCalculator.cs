namespace Core.Expense
{
    public interface ICapExExpenseEstimator
    {
        public double CalculateEstimatedMonthlyCapEx(double propertyValue, int propertyAge);
    }
}
