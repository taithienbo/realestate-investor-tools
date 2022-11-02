using Core.Dto;
using Core.Expense;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace reit_zillow_api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private IExpenseEstimator _expenseEstimator;

        public ExpenseController(IExpenseEstimator expenseEstimator)
        {
            _expenseEstimator = expenseEstimator;
        }

        [HttpGet]
        public ExpenseDetail EstimateExpenses([FromQuery] EstimateExpensesRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            return _expenseEstimator.EstimateExpenses(request);
        }
    }
}
