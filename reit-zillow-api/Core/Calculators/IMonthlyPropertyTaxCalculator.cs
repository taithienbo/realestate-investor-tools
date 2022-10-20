

namespace Core.Calculators
{
    public interface IMonthlyPropertyTaxCalculator
    {
        public double Calculate(double purchasePrice, double estimatedPropertyTaxRate);
    }
}
