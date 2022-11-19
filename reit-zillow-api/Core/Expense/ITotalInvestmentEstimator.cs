using Core.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Expense
{
    public interface ITotalInvestmentEstimator
    {
        public double EstimateTotalInvestment(double purchasePrice, double downPaymentPercent = OutOfPocketInvestmentCost.DefaultDownPaymentPercent, double closingCost = OutOfPocketInvestmentCost.DefaultDownPaymentPercent);
    }
}
