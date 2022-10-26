﻿using Core.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ExpenseEstimator
{
    public interface IExpensesCalculator
    {
        public ExpenseDetail CalculateExpenses(ListingDetail listingDetail, LoanDetail loanDetail);
    }
}