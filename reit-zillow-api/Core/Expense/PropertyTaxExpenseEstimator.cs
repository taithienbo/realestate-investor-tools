namespace Core.Expense
{
    public class PropertyTaxExpenseEstimator : IPropertyTaxExpenseEstimator
    {
        public double Calculate(double purchasePrice, double estimatedPropertyTaxRate)
        {
            return purchasePrice * estimatedPropertyTaxRate / 100 / 12;
        }
    }
}
