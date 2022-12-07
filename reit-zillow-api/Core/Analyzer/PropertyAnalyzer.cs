using Core.Constants;
using Core.Dto;
using Core.Expense;
using Core.Income;
using Core.Interest;
using Core.Listing;
using Core.Options;
using Core.Zillow;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Analyzer
{
    public class PropertyAnalyzer : IPropertyAnalyzer
    {
        private readonly IPriceRentalParser _priceRentalParser;
        private readonly IZillowClient _zillowClient;
        private readonly IExpenseEstimator _expenseEstimator;
        private readonly IListingParser _listingParser;
        private readonly IMortgageInterestEstimator _mortgageInterestEstimator;
        private readonly ITotalInvestmentEstimator _outOfPocketCostEstimator;
        private readonly AppOptions _appOptions;

        public PropertyAnalyzer(IPriceRentalParser priceRentalParser,
            IZillowClient zillowClient,
            IListingParser listingParser,
            IMortgageInterestEstimator mortgageInterestEstimator,
            IExpenseEstimator expenseEstimator,
            ITotalInvestmentEstimator outOfPocketCostEstimator,
            AppOptions appOptions)
        {
            _priceRentalParser = priceRentalParser;
            _zillowClient = zillowClient;
            _listingParser = listingParser;
            _mortgageInterestEstimator = mortgageInterestEstimator;
            _expenseEstimator = expenseEstimator;
            _outOfPocketCostEstimator = outOfPocketCostEstimator;
            _appOptions = appOptions;
        }

        public async Task<PropertyAnalysisDetail?> AnalyzeProperty(string address)
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
                RentAmount = rentAmount,
                HoaFee = listingDetail.HoaFee
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
