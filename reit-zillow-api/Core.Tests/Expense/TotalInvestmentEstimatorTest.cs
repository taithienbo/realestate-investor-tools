using Core.Expense;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Tests.Expense
{
    public class TotalInvestmentEstimatorTest
    {
        private ITotalInvestmentEstimator _outOfPocketCostEstimator;

        public TotalInvestmentEstimatorTest()
        {
            _outOfPocketCostEstimator = new TotalInvestmentEstimator();
        }
        [Fact]
        public void EstimateOutOfPocketCost()
        {
            // arrange 
            var purchasePrice = 565000;
            var downPaymentPercent = 25;
            var closingCosts = 15000;
            // act 
            var outOfPocketCost = _outOfPocketCostEstimator.EstimateTotalInvestment(purchasePrice, downPaymentPercent, closingCosts);

            // assuming 25% down payment + 15000 closing cost
            // assert 
            var expectedOutOfPocketCost = 156250;
            Assert.Equal(expectedOutOfPocketCost, outOfPocketCost, 0);


        }
    }
}
