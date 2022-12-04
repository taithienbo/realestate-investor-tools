using Core.Constants;
using Core.Dto;
using Core.Expense;
using Core.Interest;
using Core.Loan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Amortization
{
    public class AmortizationScheduleCalculator : IAmortizationScheduleCalculator
    {
        private readonly IMortgageExpenseEstimator _mortgageExpenseEstimator;

        public AmortizationScheduleCalculator(IMortgageExpenseEstimator mortgageExpenseEstimator)
        {
            _mortgageExpenseEstimator = mortgageExpenseEstimator;
        }

        public AmortizationScheduleEntry[] Calculate(double startingPrinciple, double interestRate,
            DateTime loanStartDate, LoanProgram loanProgram)
        {
            double monthlyPayment = _mortgageExpenseEstimator.Calculate(startingPrinciple, interestRate, loanProgram);
            double remainingBalance = startingPrinciple;
            int year = loanStartDate.Year;
            int month = loanStartDate.Month;
            AmortizationScheduleEntry[] entries = new AmortizationScheduleEntry[loanProgram.NumberOfMonths()];

            for (int i = 0; i < loanProgram.NumberOfMonths(); i++)
            {
                double interest = (remainingBalance * interestRate / 100) / 12;
                double principal = monthlyPayment - interest;
                remainingBalance = remainingBalance - principal;
                entries[i] = new AmortizationScheduleEntry()
                {
                    Interest = interest,
                    Principal = principal,
                    Month = month,
                    Year = year,
                    RemainingBalance = remainingBalance
                };
                if (month == 12)
                {
                    year++;
                    month = 1;
                }
                else
                {
                    month++;
                }
            }
            return entries;
        }
    }
}
