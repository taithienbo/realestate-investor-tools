using Core.Calculators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Calculator
{
    public class CapExCalculator : ICapExCalculator
    {
        private const double BaseCapExPercentOfPropertyValue = .20;

        public double CalculateEstimatedMonthlyCapEx(double propertyValue, int propertyAge)
        {
            var monthlyBaseAmount = BaseCapExPercentOfPropertyValue / 100 * propertyValue / 12;
            double monthlyAdditionalAmountBasedOnAge = propertyAge;
            var estimatedMonthlyAmount = monthlyBaseAmount + monthlyAdditionalAmountBasedOnAge;
            return estimatedMonthlyAmount;
        }
    }
}
