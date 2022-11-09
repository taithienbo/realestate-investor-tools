using Core.ConsumerFinance;
using Core.Expense;
using Core.Income;
using Core.Interest;
using Core.Listing;
using Core.Zillow;
using Infrastructure.ConsumerFinance;
using Infrastructure.Expense;
using Infrastructure.Income;
using Infrastructure.Interest;
using Infrastructure.Listing;
using Infrastructure.Zillow;
using reit_zillow_api.JsonConverters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new DoubleConverter());
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient<IZillowClient>();
builder.Services.AddSingleton<IZillowClient, ZillowClient>();
builder.Services.AddSingleton<IListingParser, ListingParser>();
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


app.Run();

public partial class Program { }

