using Core.Calculators;
using Core.Dto;


namespace Infrastructure.Calculator
{
    public class MonthlyExpenseCalculator : IMonthlyExpenseCalculator
    {
        private IMonthlyMortgageCalculator _mortgageCalculator;
        private IMonthlyPropertyTaxCalculator _taxCalculator;

        public MonthlyExpenseCalculator(IMonthlyMortgageCalculator mortgageCalculator,
            IMonthlyPropertyTaxCalculator taxCalculator)
        {
            _mortgageCalculator = mortgageCalculator;
            _taxCalculator = taxCalculator;
        }

        public ExpenseDetail CalculateExpenses(ListingDetail listingDetail, LoanDetail loanDetail)
        {
            var mortgagePrincipal = listingDetail.ListingPrice
                - (listingDetail.ListingPrice * loanDetail.DownPaymentPercent / 100);

            return new ExpenseDetail()
            {
                Mortgage = _mortgageCalculator.Calculate(mortgagePrincipal,
                loanDetail.InterestRate, loanDetail.LoanProgram),
                PropertyTax = _taxCalculator.Calculate(listingDetail.ListingPrice)
            };
        }
    }
}
