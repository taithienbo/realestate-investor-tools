using Core.ExpenseEstimator;
using Core.Dto;


namespace Infrastructure.ExpenseEstimator
{
    public class EspenseEstimator : IExpensesCalculator
    {
        private IMortgageExpenseEstimator _mortgageExpenseEstimator;
        private IPropertyTaxExpenseEstimator _taxExpenseEstimator;
        private IHomeOwnerInsuranceExpenseEstimator _homeOwnerInsuranceExpenseEstimator;
        private ICapExExpenseEstimator _capExExpenseEstimator;
        private IRepairExpenseEstimator _repairExpenseEstimator;

        public EspenseEstimator(IMortgageExpenseEstimator mortgageCalculator,
            IPropertyTaxExpenseEstimator taxCalculator,
            IHomeOwnerInsuranceExpenseEstimator homeOwnerInsuranceCalculator,
            ICapExExpenseEstimator capExCalculator,
            IRepairExpenseEstimator repairExpenseEstimator)
        {
            _mortgageExpenseEstimator = mortgageCalculator;
            _taxExpenseEstimator = taxCalculator;
            _homeOwnerInsuranceExpenseEstimator = homeOwnerInsuranceCalculator;
            _capExExpenseEstimator = capExCalculator;
            _repairExpenseEstimator = repairExpenseEstimator;
        }

        public ExpenseDetail CalculateExpenses(ListingDetail listingDetail, LoanDetail loanDetail)
        {
            var mortgagePrincipal = listingDetail.ListingPrice
                - (listingDetail.ListingPrice * loanDetail.DownPaymentPercent / 100);

            return new ExpenseDetail()
            {
                Mortgage = _mortgageExpenseEstimator.Calculate(mortgagePrincipal,
                loanDetail.InterestRate, loanDetail.LoanProgram),
                PropertyTax = _taxExpenseEstimator.Calculate(listingDetail.ListingPrice),
                HomeOwnerInsurance = _homeOwnerInsuranceExpenseEstimator.CalculateMonthlyAmount(listingDetail.ListingPrice),
                CapitalExpenditures = _capExExpenseEstimator.CalculateEstimatedMonthlyCapEx(listingDetail.ListingPrice,
                listingDetail.PropertyAge),
                Repairs = _repairExpenseEstimator.EstimateMonthlyAmount(listingDetail.PropertyAge)
            };
        }
    }
}
