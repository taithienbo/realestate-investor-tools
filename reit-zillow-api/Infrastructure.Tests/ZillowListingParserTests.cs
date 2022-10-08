using Core.Listing;
using Infrastructure.Listing;

namespace Infrastructure.Tests
{
    public class ZillowListingParserTests
    {
        private readonly IListingParser _listingParser;

        public ZillowListingParserTests()
        {
            _listingParser = new ZillowListingParser();
        }

        [Fact]
        public void GetListingDetail()
        {
            const decimal expectedListingPrice = 750000;
            const int expectedNumOfBedrooms = 4;
            const int expectedNumOfBathrooms = 2;
            // arrange 
            string html = File.ReadAllText("TestFiles" + Path.DirectorySeparatorChar + "zillow_listing_1.html");
            // act 
            var listingDetail = _listingParser.Parse(html);
            // assert 
            Assert.NotNull(listingDetail);
            Assert.Equal(expectedListingPrice, listingDetail.ListingPrice);
            Assert.Equal(expectedNumOfBedrooms, listingDetail.NumOfBedrooms);
            Assert.Equal(expectedNumOfBathrooms, listingDetail.NumOfBathrooms);
        }

        [Fact]
        public void GetListingDetail_ThrowErrorOnNullArgument()
        {
            // arrange
        }
    }
}