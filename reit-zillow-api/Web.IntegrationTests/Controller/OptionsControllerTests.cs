using Core.Loan;
using Core.Options;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Web.IntegrationTests.Controller
{
    public class OptionsControllerTests
        : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public OptionsControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async void GetConfigs()
        {
            // arrange 
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/options");
            // assert 
            response.EnsureSuccessStatusCode();
            var options = await response.Content.ReadFromJsonAsync<AppOptions>();
            Assert.NotNull(options);
            Assert.True(options!.DefaultDownPaymentPercent > 0);
            Assert.True(options!.DefaultClosingCostOnBuy > 0);
            Assert.True(options!.DefaultClosingCostOnSell > 0);
            Assert.True(options!.BaseMiscExpenseMonthlyAmount > 0);
            Assert.True(options!.BaseRepairMonthlyAmount > 0);
            Assert.True(options!.BaseCapExPercentOfPropertyValue > 0);
            Assert.True(options!.BaseHomeOwnerInsurancePercentageOfPropertyValue > 0);

        }
    }
}
