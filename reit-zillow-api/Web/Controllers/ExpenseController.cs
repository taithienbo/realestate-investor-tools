using Core.Constants;
using Core.Dto;
using Core.Expense;
using Core.Income;
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
        private readonly IPriceRentalParser _priceRentalParser;

        public ExpenseController(IExpenseEstimator expenseEstimator,
            IZillowClient zillowClient,
            IListingParser listingParser,
            IPriceRentalParser priceRentalParser)
        {
            _expenseEstimator = expenseEstimator;
            _listingParser = listingParser;
            _zillowClient = zillowClient;
            _priceRentalParser = priceRentalParser;
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

        [HttpGet("{address}")]
        public async Task<ExpenseDetail> EstimateExpenses(string address)
        {
            var listingDetail = await GetListingDetail(address);
            var priceRentalDetail = await GetPriceRentalDetail(address);
            var estimateExpenseRequest = new EstimateExpensesRequest()
            {
                PropertyValue = listingDetail.ListingPrice,
                InterestRate = 6.5,
                PropertyAge = listingDetail.PropertyAge,
                LoanProgram = LoanProgram.ThirtyYearFixed.ToString(),
                RentAmount = priceRentalDetail.ZEstimate
            };
            return _expenseEstimator.EstimateExpenses(estimateExpenseRequest);
        }

        private async Task<ListingDetail> GetListingDetail(string address)
        {
            var listingDetailHtml = await _zillowClient.GetListingHtmlPage(address);
            return _listingParser.Parse(listingDetailHtml);
        }

        private async Task<PriceRentalDetail> GetPriceRentalDetail(string address)
        {
            var priceRentalDetailHtml = await _zillowClient.GetPriceMyRentalHtmlPage(address);
            return _priceRentalParser.Parse(priceRentalDetailHtml);
        }
    }
}
