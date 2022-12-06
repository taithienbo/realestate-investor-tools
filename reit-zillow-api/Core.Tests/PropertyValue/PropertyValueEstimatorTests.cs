using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.PropertyValue;

namespace Core.Tests.PropertyValue
{
    public class PropertyValueEstimatorTests
    {
        [Fact]
        public void EstimatePropertyValueAfter()
        {
            // arrange
            const double CurrentPropertyValue = 565000;
            const double ExpectedYearlyPercentageIncrease = 4.00;
            // act 
            var propertyValueEstimator = new PropertyValueEstimator(ExpectedYearlyPercentageIncrease);
            // assert
            // year 1 => 587,600
            Assert.Equal(587600, propertyValueEstimator.EvaluatePropertyValue(CurrentPropertyValue, 1), 0);
            // year 2 => 611104
            Assert.Equal(611104, propertyValueEstimator.EvaluatePropertyValue(CurrentPropertyValue, 2), 0);
            // year 3 =>  635548
            Assert.Equal(635548, propertyValueEstimator.EvaluatePropertyValue(CurrentPropertyValue, 3), 0);
            // year 4 => 660969.93
            Assert.Equal(660969.93, propertyValueEstimator.EvaluatePropertyValue(CurrentPropertyValue, 4), 0);
            // year 5 => 687408.73
            Assert.Equal(687408.73, propertyValueEstimator.EvaluatePropertyValue(CurrentPropertyValue, 5), 0);

        }
    }
}
