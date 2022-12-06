using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.PropertyValue
{
    public interface IPropertyValueEstimator
    {
        public double EvaluatePropertyValue(double currentValue, int years);
    }
}
