using Core.SellingCostOrProfit;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Tests.SellingCostOrProfit
{
    public class SellingCostEstimatorTests
    {
        [Fact]
        public void EstimateSellingCost()
        {
            // arrange 
            const double CurrentValue = 700000;
            const double PurchasedValue = 565000;
            const double AgentFeesPercentage = 6;
            const double CaptialGainTaxPercentage = 15;
            const double EstimatedClosingCosts = 10000;
            // act 
            var sellingCostEstimator = new SellingCostEstimator(AgentFeesPercentage, CaptialGainTaxPercentage, EstimatedClosingCosts);
            var totalSellingCost = sellingCostEstimator.EstimateSellingCost(PurchasedValue, CurrentValue);
            // assert 
            // double expectedAgentFees = 42000;   // CurrentValue * AgentFeesPerventage/100; 
            // 
            //  double expectedCapitalGainTax = 20250;  (CurrentValue - PurchasedValue) * CapitalGainTaxPercentage/100;

            double expectedTotalSellingCost = 72250; // expectedAgentFees + expectedCapitalGainTax + EstimatedClosingCosts 
            Assert.Equal(expectedTotalSellingCost, totalSellingCost, 0);

        }
    }
}
