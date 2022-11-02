using Core.Expense;
using Core.Dto;
using Core.Constants;

namespace Infrastructure.Expense
{
    public class ExpenseEstimator : IExpenseEstimator
    {
        private IMortgageExpenseEstimator _mortgageExpenseEstimator;
        private IPropertyTaxExpenseEstimator _taxExpenseEstimator;
        private IHomeOwnerInsuranceExpenseEstimator _homeOwnerInsuranceExpenseEstimator;
        private ICapExExpenseEstimator _capExExpenseEstimator;
        private IRepairExpenseEstimator _repairExpenseEstimator;
        private IPropertyManagementExpenseEstimator _propertyManagementExpenseEstimator;
        private IMiscExpenseEstimator _miscExpenseEstimator;

        public ExpenseEstimator(IMortgageExpenseEstimator mortgageExpenseEstimator,
            IPropertyTaxExpenseEstimator propertyTaxExpenseEstimator,
            IHomeOwnerInsuranceExpenseEstimator homeOwnerInsuranceExpenseEstimator,
            ICapExExpenseEstimator capExCalculator,
            IRepairExpenseEstimator repairExpenseEstimator,
            IPropertyManagementExpenseEstimator propertyManagementExpenseEstimator,
            IMiscExpenseEstimator miscExpenseEstimator)
        {
            _mortgageExpenseEstimator = mortgageExpenseEstimator;
            _taxExpenseEstimator = propertyTaxExpenseEstimator;
            _homeOwnerInsuranceExpenseEstimator = homeOwnerInsuranceExpenseEstimator;
            _capExExpenseEstimator = capExCalculator;
            _repairExpenseEstimator = repairExpenseEstimator;
            _propertyManagementExpenseEstimator = propertyManagementExpenseEstimator;
            _miscExpenseEstimator = miscExpenseEstimator;
        }

        public ExpenseDetail EstimateExpenses(EstimateExpensesRequest estimateExpensesRequest)
        {
            var mortgagePrincipal = estimateExpensesRequest.PropertyValue
                - (estimateExpensesRequest.PropertyValue * estimateExpensesRequest.DownPaymentPercent / 100);
            LoanProgram loanProgram;
            Enum.TryParse(estimateExpensesRequest.LoanProgram, out loanProgram);
            return new ExpenseDetail()
            {
                Mortgage = _mortgageExpenseEstimator.Calculate(mortgagePrincipal,
                estimateExpensesRequest.InterestRate, loanProgram),
                PropertyTax = _taxExpenseEstimator.Calculate(estimateExpensesRequest.PropertyValue),
                HomeOwnerInsurance = _homeOwnerInsuranceExpenseEstimator.CalculateMonthlyAmount(estimateExpensesRequest.PropertyValue),
                CapitalExpenditures = _capExExpenseEstimator.CalculateEstimatedMonthlyCapEx(estimateExpensesRequest.PropertyValue,
                estimateExpensesRequest.PropertyAge),
                Repairs = _repairExpenseEstimator.EstimateMonthlyAmount(estimateExpensesRequest.PropertyAge),
                PropertyManagement = _propertyManagementExpenseEstimator.EstimateMonthlyAmount(estimateExpensesRequest.RentAmount),
                Misc = _miscExpenseEstimator.EstimateMonthlyAmount()
            };
        }
    }
}
