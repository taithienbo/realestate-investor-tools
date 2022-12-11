using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Selling
{
    public interface ISellingCostEstimator
    {
        public double EstimateSellingCost(double purchasedValue, double currentValue);
    }
}
