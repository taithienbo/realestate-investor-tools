using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.PropertyValue
{
    public class PropertyValueEstimator : IPropertyValueEstimator
    {
        private readonly double _expectedYearlyPercentageIncrease;

        public PropertyValueEstimator(double expectedYearlyPercentageIncrease)
        {
            _expectedYearlyPercentageIncrease = expectedYearlyPercentageIncrease;
        }

        public double EvaluatePropertyValue(double currentValue, int years)
        {
            double value = currentValue;
            for (int i = 0; i < years; i++)
            {
                value += value * _expectedYearlyPercentageIncrease / 100;
            }
            return value;
        }
    }
}
