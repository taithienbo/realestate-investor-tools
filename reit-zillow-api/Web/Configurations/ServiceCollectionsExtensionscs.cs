using Core.Amortization;
using Core.Analyzer;
using Core.CashFlow;
using Core.ConsumerFinance;
using Core.Expense;
using Core.Income;
using Core.Interest;
using Core.Listing;
using Core.Options;
using Core.PropertyValue;
using Core.Selling;
using Core.Zillow;
using Infrastructure.ConsumerFinance;
using Infrastructure.Income;
using Infrastructure.Listing;

namespace reit_zillow_api.Configurations
{
    public static class WebApplicationBuilderExtensions
    {
        public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton(builder.Configuration.GetSection(AppOptions.App).Get<AppOptions>());
            builder.Services.AddSingleton<IZillowListingParser, ZillowListingParser>();
            builder.Services.AddSingleton<IExpenseEstimator, ExpenseEstimator>();
            builder.Services.AddSingleton<IMortgageExpenseEstimator, MortgageExpenseEstimator>();
            builder.Services.AddSingleton<IPropertyTaxExpenseEstimator, PropertyTaxExpenseEstimator>();
            builder.Services.AddSingleton<IHomeOwnerInsuranceExpenseEstimator, HomeOwnerInsuranceExpenseEstimator>();
            builder.Services.AddSingleton<ICapExExpenseEstimator, CapExExpenseEstimator>();
            builder.Services.AddSingleton<IRepairExpenseEstimator, RepairExpenseEstimator>();
            builder.Services.AddSingleton<IPropertyManagementExpenseEstimator, PropertyManagementExpenseEstimator>();
            builder.Services.AddSingleton<IMiscExpenseEstimator, MiscExpenseEstimator>();
            builder.Services.AddSingleton<IPriceRentalParser, PriceRentalParser>();
            builder.Services.AddSingleton<IMortgageInterestService, MortgageInterestEstimator>();
            builder.Services.AddHttpClient<IRateCheckerApiClient>();
            builder.Services.AddSingleton<IRateCheckerApiClient, RateCheckerApiClient>();
            builder.Services.AddSingleton<ITotalInvestmentEstimator, TotalInvestmentEstimator>();
            builder.Services.AddSingleton<IPropertiesAnalyzer, PropertiesAnalyzer>();
            builder.Services.AddSingleton<IHouseSearchParser, HouseSearchParser>();
            builder.Services.AddSingleton<IFutureAnalyzer, FutureAnalyzer>();
            builder.Services.AddSingleton<IPropertyValueEstimator, PropertyValueEstimator>();
            builder.Services.AddSingleton<IAmortizationScheduleCalculator, AmortizationScheduleCalculator>();
            builder.Services.AddSingleton<ISellingCostEstimator, SellingCostEstimator>();
            builder.Services.AddSingleton<IListingService, ZillowListingService>();
            builder.Services.AddSingleton<IPriceRentalService, PriceRentalService>();
            builder.Services.AddSingleton<ICashFlowService, CashFlowService>();
            builder.Services.AddSingleton<IIncomesService, IncomesEstimator>();
            builder.Services.AddSingleton<IExpenseService, ExpenseService>();
            builder.Services.AddSingleton<IHouseSearchService, HouseSearchService>();

            builder.Services.AddMemoryCache();
            return builder;
        }
    }
}
