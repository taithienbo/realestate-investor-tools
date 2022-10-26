using Core.ExpenseEstimator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ExpenseEstimator
{
    public class PropertyManagementExpenseEstimator : IPropertyManagementExpenseEstimator
    {
        private const double BaseMonthlyPropertyMangementCostAsPercentageOfRentAmount = 5.00;

        public double EstimateMonthlyAmount(double rentAmount)
        {
            return BaseMonthlyPropertyMangementCostAsPercentageOfRentAmount / 100 * rentAmount;
        }
    }
}
