using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.SellingCostOrProfit
{
    public class SellingCostEstimator : ISellingCostEstimator
    {
        private readonly double _agentFeesPercentage;
        private readonly double _capitalTaxGainPercentage;
        private readonly double _estimatedClosingCosts;
        public SellingCostEstimator(double agentFeesPercentage, double capitalTaxGainPercentage, double estimatedClosingCosts)
        {
            _agentFeesPercentage = agentFeesPercentage;
            _capitalTaxGainPercentage = capitalTaxGainPercentage;
            _estimatedClosingCosts = estimatedClosingCosts;
        }

        public double EstimateSellingCost(double purchasedValue, double currentValue)
        {
            double agentFees = currentValue * _agentFeesPercentage / 100;
            double capitalGainTax = (currentValue - purchasedValue) * _capitalTaxGainPercentage / 100;
            return _estimatedClosingCosts + agentFees + capitalGainTax;
        }
    }
}
