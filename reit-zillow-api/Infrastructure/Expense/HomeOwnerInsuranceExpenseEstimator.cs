using Core.Expense;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Expense
{
    public class HomeOwnerInsuranceExpenseEstimator
        : IHomeOwnerInsuranceExpenseEstimator
    {
        private const double EstimateHomeOwnerInsurancePercentageOfListingPrice = .25;

        public double CalculateMonthlyAmount(double listingPrice)
        {
            return listingPrice * EstimateHomeOwnerInsurancePercentageOfListingPrice / 100 / 12;
        }
    }
}
