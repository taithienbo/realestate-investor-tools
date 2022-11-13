using Core.Dto;

namespace Core.Expense
{
    public interface IExpenseEstimator
    {
        public IDictionary<string, double> EstimateExpenses(EstimateExpensesRequest estimateExpensesRequest);
    }
}
