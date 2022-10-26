using Core.ExpenseEstimator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ExpenseEstimator
{
    public class RepairExpenseEstimator : IRepairExpenseEstimator
    {
        private const double BaseMonthlyAmount = 110;

        public double EstimateMonthlyAmount(int propertyAge)
        {
            return BaseMonthlyAmount + propertyAge;
        }
    }
}
