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
        private readonly AppOptions _appOptions;
        private readonly IMortgageInterestService _mortgageInterestEstimator;
        private readonly ITotalInvestmentEstimator _outOfPocketCostEstimator;
        private readonly IExpenseService _expenseService;
        private readonly IListingService _listingService;
        private readonly IIncomesService _incomesService;
        private readonly IHouseSearchService _houseSearchService;

        public PropertiesAnalyzer(
           IMortgageInterestService mortgageInterestEstimator,
           ITotalInvestmentEstimator outOfPocketCostEstimator,
           IExpenseService expenseService,
           IIncomesService incomesService,
           IListingService listingService,
           IHouseSearchService houseSearchService,
        AppOptions appOptions)
        {
            _mortgageInterestEstimator = mortgageInterestEstimator;
            _outOfPocketCostEstimator = outOfPocketCostEstimator;
            _expenseService = expenseService;
            _listingService = listingService;
            _incomesService = incomesService;
            _houseSearchService = houseSearchService;
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

            analysisDetail.Incomes = await _incomesService.EstimateIncomes(address);

            analysisDetail.InterestRate = await _mortgageInterestEstimator.GetCurrentInterest(listingDetail.ListingPrice);

            analysisDetail.Expenses = await _expenseService.CalculateExpenses(address);

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

            IList<string> addresses = await _houseSearchService.SearchListings(zipCode);
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
    }
}
