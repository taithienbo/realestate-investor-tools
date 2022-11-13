namespace Core.Expense
{
    public interface IHomeOwnerInsuranceExpenseEstimator
    {
        public double CalculateMonthlyAmount(double listingPrice);
    }
}
