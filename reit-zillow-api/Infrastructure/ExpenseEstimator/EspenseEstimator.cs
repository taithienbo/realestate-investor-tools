using Core.ExpenseEstimator;
using Core.Dto;


namespace Infrastructure.ExpenseEstimator
{
    public class EspenseEstimator : IExpenseEstimator
    {
        private IMortgageExpenseEstimator _mortgageExpenseEstimator;
        private IPropertyTaxExpenseEstimator _taxExpenseEstimator;
        private IHomeOwnerInsuranceExpenseEstimator _homeOwnerInsuranceExpenseEstimator;
        private ICapExExpenseEstimator _capExExpenseEstimator;
        private IRepairExpenseEstimator _repairExpenseEstimator;
        private IPropertyManagementExpenseEstimator _propertyManagementExpenseEstimator;

        public EspenseEstimator(IMortgageExpenseEstimator mortgageExpenseEstimator,
            IPropertyTaxExpenseEstimator propertyTaxExpenseEstimator,
            IHomeOwnerInsuranceExpenseEstimator homeOwnerInsuranceExpenseEstimator,
            ICapExExpenseEstimator capExCalculator,
            IRepairExpenseEstimator repairExpenseEstimator,
            IPropertyManagementExpenseEstimator propertyManagementExpenseEstimator)
        {
            _mortgageExpenseEstimator = mortgageExpenseEstimator;
            _taxExpenseEstimator = propertyTaxExpenseEstimator;
            _homeOwnerInsuranceExpenseEstimator = homeOwnerInsuranceExpenseEstimator;
            _capExExpenseEstimator = capExCalculator;
            _repairExpenseEstimator = repairExpenseEstimator;
            _propertyManagementExpenseEstimator = propertyManagementExpenseEstimator;
        }

        public ExpenseDetail CalculateExpenses(ListingDetail listingDetail, LoanDetail loanDetail, double estimatedRentAmount)
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
                Repairs = _repairExpenseEstimator.EstimateMonthlyAmount(listingDetail.PropertyAge),
                PropertyManagement = _propertyManagementExpenseEstimator.EstimateMonthlyAmount(estimatedRentAmount)
            };
        }
    }
}
