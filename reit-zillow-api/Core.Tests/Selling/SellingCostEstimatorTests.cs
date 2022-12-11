using Core.Options;
using Core.Selling;


namespace Core.Tests.Selling
{
    public class SellingCostEstimatorTests
    {
        [Fact]
        public void EstimateSellingCost()
        {
            // arrange 

            const double CurrentValue = 700000;
            const double PurchasedValue = 565000;


            AppOptions appOptions = new AppOptions()
            {
                DefaultAgentFeesPercentageOfSellingPrice = 6,
                DefaultTaxPercentageOnSell = 15,
                DefaultClosingCostOnSell = 15000,
                DefaultRepairCostOnSell = 5000
            };
            // act 
            var sellingCostEstimator = new SellingCostEstimator(appOptions);
            var totalSellingCost = sellingCostEstimator.EstimateSellingCost(PurchasedValue, CurrentValue);
            // assert 
            // double expectedAgentFees = 42000;   // CurrentValue * AgentFeesPercentage/100; 
            // 
            //  double expectedCapitalGainTax = 20250;  (CurrentValue - PurchasedValue) * CapitalGainTaxPercentage/100;

            double expectedTotalSellingCost = 82250; // expectedAgentFees + expectedCapitalGainTax + DefaultClosingCostOnSell + DefaultRepairCostOnSell
            Assert.Equal(expectedTotalSellingCost, totalSellingCost, 0);

        }
    }
}
