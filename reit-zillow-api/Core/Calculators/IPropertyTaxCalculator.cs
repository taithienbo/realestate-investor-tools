

namespace Core.Calculators
{
    public interface IPropertyTaxCalculator
    {
        public double Calculate(double purchasePrice, double estimatedPropertyTaxRate = 1.25);
    }
}
