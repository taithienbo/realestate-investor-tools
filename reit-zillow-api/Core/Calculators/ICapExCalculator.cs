using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Calculators
{
    public interface ICapExCalculator
    {
        public double CalculateEstimatedMonthlyCapEx(double propertyValue, int propertyAge);
    }
}
