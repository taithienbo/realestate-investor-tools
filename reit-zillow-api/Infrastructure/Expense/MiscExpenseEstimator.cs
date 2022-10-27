using Core.Expense;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Expense
{
    public class MiscExpenseEstimator : IMiscExpenseEstimator
    {
        private const double BaseMiscExpenseEstimatedMonthlyAmount = 100;

        public double EstimateMonthlyAmount()
        {
            return BaseMiscExpenseEstimatedMonthlyAmount;
        }
    }
}
