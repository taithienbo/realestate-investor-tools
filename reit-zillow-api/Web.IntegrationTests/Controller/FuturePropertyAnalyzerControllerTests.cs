using Core.Loan;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.OpenApi.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.IntegrationTests.Controller
{
    public class FuturePropertyAnalyzerControllerTests
        : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public FuturePropertyAnalyzerControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async void AnalyzeInvestmentByForm()
        {
            // arrange 
            const double DownPaymentAmount = 60000;
            const double OriginalLoanAmount = 432000;
            const double OriginalPurchaseAmount = 570000;
            const double HoldingPeriodInYears = 5;

            var client = _factory.CreateClient();
            var queryParameters = new Dictionary<string, string?>()
            {
                ["DownPaymentAmount"] = DownPaymentAmount.ToString(),
                ["OriginalLoanAmount"] = OriginalLoanAmount.ToString(),
                ["OriginalPurchaseAmount"] = OriginalPurchaseAmount.ToString(),
                ["HoldingPeriodInYears"] = HoldingPeriodInYears.ToString(),
                ["LoanProgram"] = LoanProgram.ThirtyYearFixed.ToString()
            };
            var uri = QueryHelpers.AddQueryString("/futurepropertyanalyzer", queryParameters);
            // act 
            var response = await client.GetAsync(uri);
            // assert 
            response.EnsureSuccessStatusCode();
        }


    }
}
