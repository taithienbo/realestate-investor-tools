using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.IntegrationTests.Controller
{
    public class PropertyAnalyzerControllerTests
        : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public PropertyAnalyzerControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async void Status()
        {
            // arrange 
            var client = _factory.CreateClient();
            // act 
            var response = await client.GetAsync("/propertyanalyzer");
            // assert 
            response.EnsureSuccessStatusCode();
        }
    }
}
