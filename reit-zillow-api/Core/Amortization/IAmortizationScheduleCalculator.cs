using Core.Constants;
using Core.Dto;
using Core.Loan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Amortization
{
    public interface IAmortizationScheduleCalculator
    {
        public AmortizationScheduleEntry[] Calculate(double startingPrinciple, double interestRate, DateTime loanStartDate,
            LoanProgram loanProgram = LoanProgram.ThirtyYearFixed);
    }
}
