﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Expense
{
    public interface IPropertyManagementExpenseEstimator
    {
        public double EstimateMonthlyAmount(double rentAmount);
    }
}