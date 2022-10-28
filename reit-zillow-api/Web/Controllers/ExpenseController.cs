using Core.Dto;
using Core.Expense;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace reit_zillow_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private IExpenseEstimator _expenseEstimator;

        public ExpenseController(IExpenseEstimator expenseEstimator)
        {
            _expenseEstimator = expenseEstimator;
        }

        public ExpenseDetail EstimateExpenses(EstimateExpensesRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            return _expenseEstimator.EstimateExpenses(request);
        }
    }
}
