using Core.Calculators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Calculator
{
    public class MonthlyPropertyTaxCalculator : IMonthlyPropertyTaxCalculator
    {
        public double Calculate(double purchasePrice, double estimatedPropertyTaxRate)
        {
            return purchasePrice * estimatedPropertyTaxRate / 100 / 12;
        }
    }
}
