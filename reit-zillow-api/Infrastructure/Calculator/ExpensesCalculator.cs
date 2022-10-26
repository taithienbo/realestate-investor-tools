using Core.Calculators;
using Core.Dto;


namespace Infrastructure.Calculator
{
    public class ExpensesCalculator : IExpensesCalculator
    {
        private IMortgageCalculator _mortgageCalculator;
        private IPropertyTaxCalculator _taxCalculator;
        private IHomeOwnerInsuranceCalculator _homeOwnerInsuranceCalculator;
        private ICapExCalculator _capExCalculator;

        public ExpensesCalculator(IMortgageCalculator mortgageCalculator,
            IPropertyTaxCalculator taxCalculator,
            IHomeOwnerInsuranceCalculator homeOwnerInsuranceCalculator,
            ICapExCalculator capExCalculator)
        {
            _mortgageCalculator = mortgageCalculator;
            _taxCalculator = taxCalculator;
            _homeOwnerInsuranceCalculator = homeOwnerInsuranceCalculator;
            _capExCalculator = capExCalculator;
        }

        public ExpenseDetail CalculateExpenses(ListingDetail listingDetail, LoanDetail loanDetail)
        {
            var mortgagePrincipal = listingDetail.ListingPrice
                - (listingDetail.ListingPrice * loanDetail.DownPaymentPercent / 100);

            return new ExpenseDetail()
            {
                Mortgage = _mortgageCalculator.Calculate(mortgagePrincipal,
                loanDetail.InterestRate, loanDetail.LoanProgram),
                PropertyTax = _taxCalculator.Calculate(listingDetail.ListingPrice),
                HomeOwnerInsurance = _homeOwnerInsuranceCalculator.CalculateMonthlyAmount(listingDetail.ListingPrice),
                CapitalExpenditures = _capExCalculator.CalculateEstimatedMonthlyCapEx(listingDetail.ListingPrice,
                listingDetail.PropertyAge)
            };
        }
    }
}
