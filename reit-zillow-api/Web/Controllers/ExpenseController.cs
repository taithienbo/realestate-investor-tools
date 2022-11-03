using Core.Constants;
using Core.Dto;
using Core.Expense;
using Core.Listing;
using Core.Zillow;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace reit_zillow_api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private readonly IExpenseEstimator _expenseEstimator;
        private readonly IListingParser _listingParser;
        private readonly IZillowClient _zillowClient;
        public ExpenseController(IExpenseEstimator expenseEstimator, IZillowClient zillowClient,
            IListingParser listingParser)
        {
            _expenseEstimator = expenseEstimator;
            _listingParser = listingParser;
            _zillowClient = zillowClient;
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

        [HttpGet]
        public async Task<ExpenseDetail> EstimateExpenses(string propertyAddress)
        {
            var htmlListingDetail = await _zillowClient.GetHtml(propertyAddress);
            var listingDetail = _listingParser.Parse(htmlListingDetail);
            var estimateExpenseRequest = new EstimateExpensesRequest()
            {
                PropertyValue = listingDetail.ListingPrice,
                InterestRate = 6.5,
                PropertyAge = listingDetail.PropertyAge,
                LoanProgram = LoanProgram.ThirtyYearFixed.ToString(),
                RentAmount = 3000
            };
            return _expenseEstimator.EstimateExpenses(estimateExpenseRequest);
        }
    }
}
