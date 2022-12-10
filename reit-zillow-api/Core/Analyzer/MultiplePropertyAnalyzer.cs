using Core.Constants;
using Core.Dto;
using Core.Expense;
using Core.Income;
using Core.Interest;
using Core.Listing;
using Core.Options;
using Core.Zillow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Analyzer
{
    public class MultiplePropertyAnalyzer : IMultiplePropertyAnalyzer
    {
        private readonly IZillowClient _zillowClient;
        private readonly IHouseSearchParser _houseSearchParser;
        private readonly IListingParser _listingParser;
        private readonly AppOptions _appOptions;
        private readonly IPriceRentalParser _priceRentalParser;
        private readonly IMortgageInterestEstimator _mortgageInterestEstimator;
        private readonly ITotalInvestmentEstimator _outOfPocketCostEstimator;
        private readonly IExpenseEstimator _expenseEstimator;

        public MultiplePropertyAnalyzer(IZillowClient zillowClient,
           IHouseSearchParser houseSearchParser,
           IListingParser listingParser,
           IPriceRentalParser priceRentalParser,
           IMortgageInterestEstimator mortgageInterestEstimator,
           ITotalInvestmentEstimator outOfPocketCostEstimator,
           IExpenseEstimator expenseEstimator,
        AppOptions appOptions)
        {
            _zillowClient = zillowClient;
            _houseSearchParser = houseSearchParser;
            _listingParser = listingParser;
            _priceRentalParser = priceRentalParser;
            _mortgageInterestEstimator = mortgageInterestEstimator;
            _outOfPocketCostEstimator = outOfPocketCostEstimator;
            _expenseEstimator = expenseEstimator;
            _appOptions = appOptions;
        }

        public async Task<PropertyAnalysisDetail?> Analyze(string address)
        {
            var listingDetail = await GetListingDetail(address);
            if (listingDetail == null)
            {
                return null;
            }
            var analysisDetail = new PropertyAnalysisDetail()
            {
                AssumedDownPaymentPercent = _appOptions.DefaultDownPaymentPercent,
                AssumedClosingCost = _appOptions.DefaultClosingCostOnBuy
            };

            analysisDetail.ListingDetail = listingDetail;

            double rentalIncome = await GetRentalIncome(address);
            analysisDetail.AddIncome(nameof(CommonIncomeType.Rental), rentalIncome);

            double currentInterest = await _mortgageInterestEstimator.GetCurrentInterest(listingDetail.ListingPrice);
            analysisDetail.InterestRate = currentInterest;

            var expenses = GetExpenses(listingDetail, currentInterest, rentalIncome);
            analysisDetail.Expenses = expenses;

            analysisDetail.NetOperatingIncome = Calculators.CalculateNetOperatingIncome(analysisDetail.Incomes!, analysisDetail.Expenses);

            analysisDetail.CapRate = Calculators.CalculateCapRate(listingDetail.ListingPrice, analysisDetail.Incomes!);

            analysisDetail.DebtServiceCoverageRatio = Calculators.CalculateDebtServiceCoverageRatio(analysisDetail.Incomes!, analysisDetail.Expenses);

            analysisDetail.CashFlow = Calculators.CalculateCashFlow(analysisDetail.Incomes!, analysisDetail.Expenses);

            analysisDetail.AssumedOutOfPocketCosts = new Dictionary<string, double>
            {
                { nameof(CommonOutOfPocketCost.DownPayment),
                Calculators.CalculateDownPayment(listingDetail.ListingPrice, _appOptions.DefaultDownPaymentPercent)},
                { nameof (CommonOutOfPocketCost.ClosingCost),
                _appOptions.DefaultClosingCostOnBuy}
            };
            analysisDetail.CashOnCashReturn = Calculators.CalculateCashOnCashReturn(analysisDetail.Incomes!, analysisDetail.Expenses, _outOfPocketCostEstimator.EstimateTotalInvestment(listingDetail.ListingPrice, _appOptions.DefaultDownPaymentPercent, _appOptions.DefaultClosingCostOnBuy));
            return analysisDetail;
        }

        public async Task<IDictionary<string, PropertyAnalysisDetail>> AnalyzeProperties(int zipCode)
        {
            IDictionary<string, PropertyAnalysisDetail> propertiesAnalysis = new Dictionary<string, PropertyAnalysisDetail>();

            var addressesHtml = await _zillowClient.SearchListingsByZipCode(zipCode);
            IList<string> addresses = _houseSearchParser.ParseListingAddresses(addressesHtml);
            foreach (var address in addresses)
            {
                Thread.Sleep(100); // avoid sending too many requests in a short time and getting blocked. 
                var analysisResult = await Analyze(address);
                if (analysisResult != null)
                {
                    propertiesAnalysis[address] = analysisResult;
                }
            }
            return await Task.FromResult(propertiesAnalysis);
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

        private IDictionary<string, double> GetExpenses(ListingDetail listingDetail, double interestRate, double rentAmount)
        {
            return _expenseEstimator.EstimateExpenses(new EstimateExpensesRequest()
            {
                PropertyAge = listingDetail.PropertyAge,
                PropertyValue = listingDetail.ListingPrice,
                InterestRate = interestRate,
                RentAmount = rentAmount,
                HoaFee = listingDetail.HoaFee
            });
        }

    }


}
