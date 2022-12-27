using Core.Constants;
using Core.Dto;
using Core.Expense;
using Core.Income;
using Core.Interest;
using Core.Listing;
using Core.Loan;
using Core.Zillow;
using Microsoft.AspNetCore.Mvc;

namespace reit_zillow_api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private readonly IExpenseEstimator _expenseEstimator;
        private readonly IExpenseService _expenseService;

        public ExpenseController(IExpenseEstimator expenseEstimator,
            IExpenseService expenseService)
        {
            _expenseEstimator = expenseEstimator;
            _expenseService = expenseService;
        }

        [HttpGet]
        public IDictionary<string, double> EstimateExpenses([FromQuery] EstimateExpensesRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            return _expenseEstimator.EstimateExpenses(request);
        }

        [HttpGet("{address}")]
        public async Task<IDictionary<string, double>> EstimateExpenses(string address)
        {
            return await _expenseService.CalculateExpenses(address);
        }

    }
}
