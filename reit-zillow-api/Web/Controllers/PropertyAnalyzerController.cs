using Core.Constants;
using Core.Dto;
using Core.Expense;
using Core.Income;
using Core.Interest;
using Core.Listing;
using Core.Zillow;
using Microsoft.AspNetCore.Mvc;

namespace reit_zillow_api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PropertyAnalyzerController : ControllerBase
    {
        private readonly IPriceRentalParser _priceRentalParser;
        private readonly IZillowClient _zillowClient;
        private readonly IExpenseEstimator _expenseEstimator;
        private readonly IListingParser _listingParser;
        private readonly IMortgageInterestEstimator _mortgageInterestEstimator;


        public PropertyAnalyzerController(IPriceRentalParser priceRentalParser,
            IZillowClient zillowClient,
            IListingParser listingParser,
            IMortgageInterestEstimator mortgageInterestEstimator,
            IExpenseEstimator expenseEstimator)
        {
            _priceRentalParser = priceRentalParser;
            _zillowClient = zillowClient;
            _listingParser = listingParser;
            _mortgageInterestEstimator = mortgageInterestEstimator;
            _expenseEstimator = expenseEstimator;
        }

        [HttpGet("{address}")]
        public async Task<PropertyAnalysisDetail?> Analyze(string address)
        {
            var listingDetail = await GetListingDetail(address);
            if (listingDetail == null)
            {
                return null;
            }
            var analysisDetail = new PropertyAnalysisDetail();
            analysisDetail.ListingDetail = listingDetail;

            double rentalIncome = await GetRentalIncome(address);
            analysisDetail.AddIncome(nameof(CommonIncomeType.Rental), rentalIncome);

            double currentInterest = await _mortgageInterestEstimator.GetCurrentInterest(listingDetail.ListingPrice);
            analysisDetail.InterestRate = currentInterest;

            var expenses = GetExpenses(listingDetail, currentInterest, rentalIncome);
            analysisDetail.Expenses = expenses;
            return analysisDetail;
        }

        private async Task<ListingDetail?> GetListingDetail(string address)
        {
            var listingDetailHtml = await _zillowClient.GetListingHtmlPage(address);
            if (listingDetailHtml == null)
            {
                return null;
            }
            return _listingParser.Parse(listingDetailHtml);
        }

        private IDictionary<string, double> GetExpenses(ListingDetail listingDetail, double interestRate, double rentAmount)
        {
            return _expenseEstimator.EstimateExpenses(new EstimateExpensesRequest()
            {
                PropertyAge = listingDetail.PropertyAge,
                PropertyValue = listingDetail.ListingPrice,
                InterestRate = interestRate,
                RentAmount = rentAmount
            });
        }

        private async Task<double> GetRentalIncome(string address)
        {
            var priceMyRentalHtmlPage = await _zillowClient.GetPriceMyRentalHtmlPage(address);
            var priceMyRentalDetail = _priceRentalParser.Parse(priceMyRentalHtmlPage);
            if (priceMyRentalDetail == null)
            {
                return 0;
            }
            return priceMyRentalDetail.ZEstimate;
        }
    }
}
