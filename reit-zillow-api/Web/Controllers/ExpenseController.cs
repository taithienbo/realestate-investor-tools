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
        private readonly IZillowListingParser _listingParser;
        private readonly IZillowClient _zillowClient;
        private readonly IPriceRentalParser _priceRentalParser;
        private readonly IMortgageInterestEstimator _mortgageInterestEstimator;

        public ExpenseController(IExpenseEstimator expenseEstimator,
            IZillowClient zillowClient,
            IZillowListingParser listingParser,
            IPriceRentalParser priceRentalParser,
            IMortgageInterestEstimator mortgageInterestEstimator)
        {
            _expenseEstimator = expenseEstimator;
            _listingParser = listingParser;
            _zillowClient = zillowClient;
            _priceRentalParser = priceRentalParser;
            _mortgageInterestEstimator = mortgageInterestEstimator;
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
            var listingDetail = await GetListingDetail(address);
            var priceRentalDetail = await GetPriceRentalDetail(address);
            var estimateExpenseRequest = new EstimateExpensesRequest()
            {
                PropertyValue = listingDetail.ListingPrice,
                InterestRate = await _mortgageInterestEstimator.GetCurrentInterest(DefaultLoanAmount(listingDetail.ListingPrice), listingDetail.ListingPrice),
                PropertyAge = listingDetail.PropertyAge,
                LoanProgram = LoanProgram.ThirtyYearFixed.ToString(),
                RentAmount = priceRentalDetail.ZEstimate,
                HoaFee = listingDetail.HoaFee
            };
            return _expenseEstimator.EstimateExpenses(estimateExpenseRequest);
        }

        private double DefaultLoanAmount(double listingPrice)
        {
            // assume 25% down payment for investment property 
            return listingPrice - .25 * listingPrice;
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
