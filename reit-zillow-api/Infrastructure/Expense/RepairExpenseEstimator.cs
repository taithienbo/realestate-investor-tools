using Core.Expense;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Expense
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
