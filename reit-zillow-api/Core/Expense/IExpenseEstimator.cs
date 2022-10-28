﻿using Core.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Expense
{
    public interface IExpenseEstimator
    {
        public ExpenseDetail EstimateExpenses(EstimateExpensesRequest estimateExpensesRequest);
    }
}