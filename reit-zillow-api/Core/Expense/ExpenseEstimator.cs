using Core.Constants;
using Core.Dto;

namespace Core.Expense
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

        public IDictionary<string, double> EstimateExpenses(EstimateExpensesRequest estimateExpensesRequest)
        {
            var mortgagePrincipal = estimateExpensesRequest.PropertyValue
                - (estimateExpensesRequest.PropertyValue * estimateExpensesRequest.DownPaymentPercent / 100);
            LoanProgram loanProgram;
            Enum.TryParse(estimateExpensesRequest.LoanProgram, out loanProgram);

            var mortgage = _mortgageExpenseEstimator.Calculate(mortgagePrincipal,
                estimateExpensesRequest.InterestRate, loanProgram);
            var propertyTax = _taxExpenseEstimator.Calculate(estimateExpensesRequest.PropertyValue);
            var homeOwnerInsureance = _homeOwnerInsuranceExpenseEstimator.CalculateMonthlyAmount(estimateExpensesRequest.PropertyValue);
            var capitalExpenditures = _capExExpenseEstimator.CalculateEstimatedMonthlyCapEx(estimateExpensesRequest.PropertyValue,
                estimateExpensesRequest.PropertyAge);
            var repairs = _repairExpenseEstimator.EstimateMonthlyAmount(estimateExpensesRequest.PropertyAge);
            var propertyManagement = _propertyManagementExpenseEstimator.EstimateMonthlyAmount(estimateExpensesRequest.RentAmount);
            var misc = _miscExpenseEstimator.EstimateMonthlyAmount();
            var expenses = new Dictionary<string, double>()
            {
                { nameof(CommonExpenseType.Mortgage), mortgage },
                { nameof(CommonExpenseType.PropertyTax), propertyTax },
                { nameof(CommonExpenseType.HomeOwnerInsurance), homeOwnerInsureance },
                { nameof(CommonExpenseType.CapitalExpenditures), capitalExpenditures },
                { nameof(CommonExpenseType.Repairs), repairs },
                { nameof(CommonExpenseType.PropertyManagement), propertyManagement },
                { nameof(CommonExpenseType.Misc), misc }
            };
            if (estimateExpensesRequest.HoaFee > 0)
            {
                expenses.Add(nameof(CommonExpenseType.HoaFee), estimateExpensesRequest.HoaFee);
            }
            return expenses;
        }
    }
}
