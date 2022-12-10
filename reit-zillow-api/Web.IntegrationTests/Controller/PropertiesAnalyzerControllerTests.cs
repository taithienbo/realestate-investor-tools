using Microsoft.AspNetCore.Mvc.Testing;


namespace Web.IntegrationTests.Controller
{
    public class PropertiesAnalyzerControllerTests
        : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public PropertiesAnalyzerControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async void Status()
        {
            // arrange 
            var client = _factory.CreateClient();
            // act 
            var response = await client.GetAsync("/propertiesanalyzer/status");
            // assert 
            response.EnsureSuccessStatusCode();
        }
    }
}
