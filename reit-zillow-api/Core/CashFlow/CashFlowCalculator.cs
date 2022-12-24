using Core.Expense;
using Core.Income;
using Core.Listing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CashFlow
{
    public class CashFlowCalculator : ICashFlowCalculator
    {
        private readonly IIncomesEstimator _incomesEstimator;
        private readonly IExpenseService _expenseService;

        public CashFlowCalculator(IIncomesEstimator incomesEstimator, IExpenseService expenseService)
        {
            _incomesEstimator = incomesEstimator;
            _expenseService = expenseService;
        }

        public async Task<double> CalculateCashFlow(string address)
        {
            IDictionary<string, double> incomes = await _incomesEstimator.EstimateIncomes(address);
            IDictionary<string, double> expenses = await _expenseService.CalculateExpenses(address);

            return Calculators.CalculateCashFlow(incomes, expenses);
        }
    }
}
