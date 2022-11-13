using Core.Constants;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.WebUtilities;
using System.Net.Http.Json;

namespace Web.IntegrationTests.Controller
{
    public class ExpenseControllerTests
        : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public ExpenseControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async void EstimateExpenses()
        {
            // arrange 
            var client = _factory.CreateClient();
            var queryParameters = new Dictionary<string, string?>()
            {
                ["PropertyValue"] = "650000",
                ["PropertyAge"] = "50",
                ["DownPaymentPercent"] = "25",
                ["InterestRate"] = "7.0",
                ["RentAmount"] = "3000",
                ["LoanProgram"] = LoanProgram.ThirtyYearFixed.ToString()
            };
            var uri = QueryHelpers.AddQueryString("/expense", queryParameters);
            // act 
            var response = await client.GetAsync(uri);
            // assert 
            response.EnsureSuccessStatusCode();
            var expenses = await response.Content.ReadFromJsonAsync<IDictionary<string, double>>();
            Assert.NotNull(expenses);
            foreach (var commonExpense in Enum.GetNames(typeof(CommonExpenseType)))
            {
                Assert.True(expenses!.ContainsKey(commonExpense));
            }
        }

        [Fact]
        public async void EstimateExpenses_Returns400IfMissingParameters()
        {
            // arrange 
            var client = _factory.CreateClient();
            // act 
            var response = await client.GetAsync("/expense");
            // assert 
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async void EstimateExpensesGivenAddress()
        {
            // arrange 
            var client = _factory.CreateClient();
            var address = "14062-Baker-St-Westminster-CA-92683";
            var uri = $"/expense/{address}";
            // act 
            var response = await client.GetAsync(uri);
            // assert 
            response.EnsureSuccessStatusCode();
            var expenses = await response.Content.ReadFromJsonAsync<IDictionary<string, double>>();
            Assert.NotNull(expenses);
            foreach (var commonExpense in Enum.GetNames(typeof(CommonExpenseType)))
            {
                Assert.True(expenses!.ContainsKey(commonExpense));
            }
        }
    }
}
