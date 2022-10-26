using Core.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ExpenseEstimator
{
    public interface IMortgageExpenseEstimator
    {
        public double Calculate(double mortgagePrincipal, double annualIntrestRate,
            LoanProgram loanProgram = LoanProgram.ThirtyYearFixed);
    }
}
