using Core.Amortization;
using Core.Analyzer;
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
using Infrastructure.Listing;
using Infrastructure.Zillow;

namespace reit_zillow_api.Configurations
{
    public static class WebApplicationBuilderExtensions
    {
        public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton(builder.Configuration.GetSection(AppOptions.App).Get<AppOptions>());
            builder.Services.AddSingleton<IZillowClient, ZillowClient>();
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
            builder.Services.AddSingleton<IMortgageInterestEstimator, MortgageInterestEstimator>();
            builder.Services.AddHttpClient<IRateCheckerApiClient>();
            builder.Services.AddSingleton<IRateCheckerApiClient, RateCheckerApiClient>();
            builder.Services.AddSingleton<ITotalInvestmentEstimator, TotalInvestmentEstimator>();
            builder.Services.AddSingleton<IPropertiesAnalyzer, PropertiesAnalyzer>();
            builder.Services.AddSingleton<IHouseSearchParser, HouseSearchParser>();
            builder.Services.AddSingleton<IFutureAnalyzer, FutureAnalyzer>();
            builder.Services.AddSingleton<IPropertyValueEstimator, PropertyValueEstimator>();
            builder.Services.AddSingleton<IAmortizationScheduleCalculator, AmortizationScheduleCalculator>();
            builder.Services.AddSingleton<ISellingCostEstimator, SellingCostEstimator>();
            builder.Services.AddSingleton<IZillowListingService, ZillowListingService>();

            return builder;
        }
    }
}
