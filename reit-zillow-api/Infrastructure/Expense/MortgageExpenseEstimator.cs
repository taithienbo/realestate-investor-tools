using Core.Expense;
using Core.Constants;

namespace Infrastructure.Expense
{
    public class MortgageExpenseEstimator : IMortgageExpenseEstimator
    {
        public double Calculate(double mortgagePrincipal, double annualInterestPercentage,
            LoanProgram loanProgram)
        {
            /**
             * M = P [ i(1 + i)^n ] / [ (1 + i)^n – 1]. 

                Here’s a breakdown of each of the variables:

                M = Total monthly payment
                P = The total amount of your loan
                I = Your interest rate, as a monthly percentage
                N = The total amount of months in your timeline for paying off your mortgage
             */
            var monthlyInterest = (annualInterestPercentage / 100) / 12;    // I 
            var totalNumOfPayments = CalculateTotalNumOfPayments(loanProgram);  // N 
            var dividend = mortgagePrincipal * (monthlyInterest * (Math.Pow(1 + monthlyInterest, totalNumOfPayments)));
            var divisor = Math.Pow(1 + monthlyInterest, totalNumOfPayments) - 1;
            return dividend / divisor;
        }

        private int CalculateTotalNumOfPayments(LoanProgram loanProgram)
        {
            switch (loanProgram)
            {
                case LoanProgram.ThirtyYearFixed:
                    return 30 * 12;
                default:
                    return 0;
            }
        }
    }
}
