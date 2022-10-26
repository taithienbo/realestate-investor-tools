using Core.ExpenseEstimator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ExpenseEstimator
{
    public class PropertyTaxExpenseEstimator : IPropertyTaxExpenseEstimator
    {
        public double Calculate(double purchasePrice, double estimatedPropertyTaxRate)
        {
            return purchasePrice * estimatedPropertyTaxRate / 100 / 12;
        }
    }
}
