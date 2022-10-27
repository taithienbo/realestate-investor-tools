using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Expense
{
    public interface IHomeOwnerInsuranceExpenseEstimator
    {
        public double CalculateMonthlyAmount(double listingPrice);
    }
}
