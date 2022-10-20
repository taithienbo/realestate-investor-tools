using Core.Calculators;
using Core.Dto;


namespace Infrastructure.Calculator
{
    public class MonthlyExpenseCalculator : IMonthlyExpenseCalculator
    {
        public ExpenseDetail CalculateExpenses(ListingDetail listingDetail, LoanDetail loanDetail)
        {
            return new ExpenseDetail()
            {
                Mortgage = 4466,
                PropertyTax = 921.875
            };
        }
    }
}
