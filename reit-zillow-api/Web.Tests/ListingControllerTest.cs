using Core.Dto;
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
        private readonly Mock<IListingService> _mockListingService;
        private readonly ListingController _listingController;

        public ListingControllerTest()
        {
            _mockListingService = new Mock<IListingService>();
            _listingController = new ListingController(_mockListingService.Object);
        }

        [Fact]
        public async void GetListingInfo()
        {

            const string Address = "829 N Lenz Dr, Anaheim, CA 92805";
            var expectedListingDetail = new ListingDetail()
            {
                ListingPrice = new Random().NextDouble()
            };
            _mockListingService.Setup(listingService => listingService.GetListingDetail(Address)).ReturnsAsync(expectedListingDetail);
            // act

            var listingDetail = await _listingController.GetListingInfo(Address);
            // assert
            Assert.NotNull(listingDetail);
            Assert.Equal(expectedListingDetail, listingDetail);
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