using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ExpenseEstimator
{
    public interface IHomeOwnerInsuranceExpenseEstimator
    {
        public double CalculateMonthlyAmount(double listingPrice);
    }
}
