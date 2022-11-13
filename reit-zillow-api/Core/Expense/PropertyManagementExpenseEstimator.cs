namespace Core.Expense
{
    public class PropertyManagementExpenseEstimator : IPropertyManagementExpenseEstimator
    {
        private const double BaseMonthlyPropertyMangementCostAsPercentageOfRentAmount = 5.00;

        public double EstimateMonthlyAmount(double rentAmount)
        {
            return BaseMonthlyPropertyMangementCostAsPercentageOfRentAmount / 100 * rentAmount;
        }
    }
}
