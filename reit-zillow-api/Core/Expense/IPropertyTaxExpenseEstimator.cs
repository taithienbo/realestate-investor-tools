

namespace Core.Expense
{
    public interface IPropertyTaxExpenseEstimator
    {
        public double Calculate(double purchasePrice, double estimatedPropertyTaxRate = 1.25);
    }
}
