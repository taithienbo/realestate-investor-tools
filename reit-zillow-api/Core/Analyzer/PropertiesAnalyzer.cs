using Core.Constants;
using Core.Dto;
using Core.Expense;
using Core.Income;
using Core.Interest;
using Core.Listing;
using Core.Options;
using Core.Zillow;


namespace Core.Analyzer
{
    public class PropertiesAnalyzer : IPropertiesAnalyzer
    {
        private readonly IZillowClient _zillowClient;
        private readonly IHouseSearchParser _houseSearchParser;
        private readonly AppOptions _appOptions;
        private readonly IMortgageInterestEstimator _mortgageInterestEstimator;
        private readonly ITotalInvestmentEstimator _outOfPocketCostEstimator;
        private readonly IExpenseEstimator _expenseEstimator;
        private readonly IListingService _listingService;
        private readonly IIncomesEstimator _incomesEstimator;

        public PropertiesAnalyzer(IZillowClient zillowClient,
           IHouseSearchParser houseSearchParser,
           IMortgageInterestEstimator mortgageInterestEstimator,
           ITotalInvestmentEstimator outOfPocketCostEstimator,
           IExpenseEstimator expenseEstimator,
           IIncomesEstimator incomesEstimator,
           IListingService listingService,
        AppOptions appOptions)
        {
            _zillowClient = zillowClient;
            _houseSearchParser = houseSearchParser;
            _mortgageInterestEstimator = mortgageInterestEstimator;
            _outOfPocketCostEstimator = outOfPocketCostEstimator;
            _expenseEstimator = expenseEstimator;
            _listingService = listingService;
            _incomesEstimator = incomesEstimator;
            _appOptions = appOptions;
        }

        public async Task<PropertyAnalysisDetail?> Analyze(string address)
        {
            var listingDetail = await _listingService.GetListingDetail(address);
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

            analysisDetail.Incomes = await _incomesEstimator.EstimateIncomes(address);

            double currentInterest = await _mortgageInterestEstimator.GetCurrentInterest(listingDetail.ListingPrice);
            analysisDetail.InterestRate = currentInterest;

            var expenses = GetExpenses(listingDetail, currentInterest, analysisDetail.Incomes[nameof(CommonIncomeType.Rental)]);
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
