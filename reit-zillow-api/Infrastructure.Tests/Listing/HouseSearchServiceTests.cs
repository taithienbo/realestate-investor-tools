using Core.Listing;
using Core.Zillow;
using Infrastructure.Listing;
using Moq;
using Moq.Protected;
using System.Net;

namespace Infrastructure.Tests.Listing
{
    public class HouseSearchServiceTests
    {
        private IHouseSearchService _houseSearchService;
        private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;
        private readonly Mock<HttpMessageHandler> _mockMessageHandler;
        private readonly Mock<IHouseSearchParser> _mockHouseSearchParser;

        public HouseSearchServiceTests()
        {
            _mockMessageHandler = new Mock<HttpMessageHandler>();
            _mockHttpClientFactory = new Mock<IHttpClientFactory>();
            _mockHttpClientFactory.Setup(factory => factory.CreateClient(It.IsAny<string>()))
                .Returns(new HttpClient(_mockMessageHandler.Object));

            _mockHouseSearchParser = new Mock<IHouseSearchParser>();

            _houseSearchService = new HouseSearchService(_mockHttpClientFactory.Object, _mockHouseSearchParser.Object);
        }

        [Fact]
        public async void SearchHousesByZipCode()
        {
            // arrange 
            const int ZipCode = 12345;
            IList<string> expectedAddresses = new string[] { "123 Test Ave", "456 Test Ave" };
            const string TestListingsHtml = "Test html";

            _mockMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(TestListingsHtml)
                });

            _mockHouseSearchParser.Setup(houseSearchParser => houseSearchParser.ParseListingAddresses(TestListingsHtml)).Returns(expectedAddresses);
            // act 
            IList<string> actualAddresses = await _houseSearchService.SearchListings(ZipCode);
            Assert.True(expectedAddresses.SequenceEqual(actualAddresses));
        }
    }
}
