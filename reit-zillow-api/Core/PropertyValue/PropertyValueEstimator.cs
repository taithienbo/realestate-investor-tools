using Core.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.PropertyValue
{
    public class PropertyValueEstimator : IPropertyValueEstimator
    {
        private readonly AppOptions _appOptions;

        public PropertyValueEstimator(AppOptions appOptions)
        {
            _appOptions = appOptions;
        }

        public double EvaluatePropertyValue(double currentValue, int years)
        {
            double value = currentValue;
            for (int i = 0; i < years; i++)
            {
                value += value * _appOptions.DefaultYearlyPercentageIncreaseInPropertyValue / 100;
            }
            return value;
        }
    }
}
