using Core.Listing;
using Core.Zillow;
using Moq;


namespace Core.Tests.Listing
{
    public class HouseSearchServiceTests
    {
        private IHouseSearchService _houseSearchService;
        private readonly Mock<IZillowClient> _mockZillowClient;
        private readonly Mock<IHouseSearchParser> _mockHouseSearchParser;

        public HouseSearchServiceTests()
        {
            _mockZillowClient = new Mock<IZillowClient>();
            _mockHouseSearchParser = new Mock<IHouseSearchParser>();

            _houseSearchService = new HouseSearchService(_mockZillowClient.Object, _mockHouseSearchParser.Object);
        }

        [Fact]
        public async void SearchHousesByZipCode()
        {
            // arrange 
            const int ZipCode = 12345;
            IList<string> expectedAddresses = new string[] { "123 Test Ave", "456 Test Ave" };
            const string TestListingsHtml = "Test html";
            _mockZillowClient.Setup(zillowClient => zillowClient.SearchListingsByZipCode(It.Is<int>(zip => zip == ZipCode))).ReturnsAsync(TestListingsHtml);
            _mockHouseSearchParser.Setup(houseSearchParser => houseSearchParser.ParseListingAddresses(TestListingsHtml)).Returns(expectedAddresses);
            // act 
            IList<string> actualAddresses = await _houseSearchService.SearchListings(ZipCode);
            Assert.True(expectedAddresses.SequenceEqual(actualAddresses));
        }
    }
}
