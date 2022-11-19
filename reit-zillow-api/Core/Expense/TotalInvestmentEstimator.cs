using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Expense
{
    public class TotalInvestmentEstimator : ITotalInvestmentEstimator
    {
        public double EstimateTotalInvestment(double purchasePrice, double downPaymentPercent = 25, double closingCost = 25)
        {
            return (downPaymentPercent / 100 * purchasePrice) + closingCost;
        }
    }
}
