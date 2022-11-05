using Core.Listing;
using Core.Zillow;
using Infrastructure.Listing;
using Microsoft.Extensions.Logging;
using Moq;
using reit_zillow_api.Controllers;

namespace Web.Tests
{
    public class ListingControllerTest
    {
        private readonly ILogger<ListingController> _mockLogger;
        private readonly IListingParser _listingParser;
        private readonly string _testFile;
        private readonly string _testHtml;
        private readonly Mock<IZillowClient> _mockZillowClient;
        private readonly ListingController _listingController;

        public ListingControllerTest()
        {
            _mockLogger = Mock.Of<ILogger<ListingController>>();
            _listingParser = new ListingParser();
            _testFile = "TestFiles" + Path.DirectorySeparatorChar + "zillow_listing_1.html";
            _testHtml = File.ReadAllText(_testFile);
            _mockZillowClient = new Mock<IZillowClient>();
            _mockZillowClient.Setup(zillowClient => zillowClient.GetListingHtmlPage(It.IsAny<string>())).ReturnsAsync(_testHtml);
            _listingController = new ListingController(_mockLogger, _mockZillowClient.Object, _listingParser);
        }

        [Fact]
        public async void GetListingInfo()
        {

            var address = "829 N Lenz Dr, Anaheim, CA 92805";
            // act
            var listingDetail = await _listingController.GetListingInfo(address);
            // assert
            Assert.NotNull(listingDetail);
            _mockZillowClient.Verify(mockZillowClient => mockZillowClient.GetListingHtmlPage(It.Is<string>(value => value.Equals(address))));
            Assert.True(listingDetail.ListingPrice > 0);
        }

        [Fact]
        public async void GetListingInfo_ThrowExceptionOnNullArgument()
        {
            // arrange
            string address = null!;
            // act, assert
            var exception = await Assert.ThrowsAnyAsync<ArgumentNullException>(() => _listingController.GetListingInfo(address!));
            Assert.NotNull(exception);
        }
    }
}