using Core.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Calculators
{
    public interface IMortgageCalculator
    {
        public double Calculate(double mortgagePrincipal, double annualIntrestRate,
            LoanProgram loanProgram = LoanProgram.ThirtyYearFixed);
    }
}
