using Core.Constants;

namespace Core.Expense
{
    public interface IMortgageExpenseEstimator
    {
        public double Calculate(double mortgagePrincipal, double annualIntrestRate,
            LoanProgram loanProgram = LoanProgram.ThirtyYearFixed);
    }
}
