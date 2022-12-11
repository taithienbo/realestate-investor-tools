using Core.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Selling
{
    public class SellingCostEstimator : ISellingCostEstimator
    {

        private readonly AppOptions _appOptions;

        public SellingCostEstimator(AppOptions appOptions)
        {
            _appOptions = appOptions;
        }

        public double EstimateSellingCost(double purchasedValue, double currentValue)
        {
            double agentFees = currentValue * _appOptions.DefaultAgentFeesPercentageOfSellingPrice / 100;
            double capitalGainTax = (currentValue - purchasedValue) * _appOptions.DefaultTaxPercentageOnSell / 100;
            return _appOptions.DefaultClosingCostOnSell + agentFees + capitalGainTax + _appOptions.DefaultRepairCostOnSell;
        }
    }
}
