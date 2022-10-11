using Core.Dto;
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
            var expectedListingDetail = new ListingDetail()
            {
                ListingPrice = 750000,
                NumOfBedrooms = 4,
                NumOfBathrooms = 2,
                NumOfStories = 1,
                NumOfParkingSpaces = 2,
                LotSizeInSqrtFt = 8849,
                NumOfGarageSpaces = 2,
                HomeType = "SingleFamily",
                PropertyCondition = "Fixer",
                YearBuilt = 1952,
                HasHOA = false
            };
            // arrange 
            string html = File.ReadAllText("TestFiles" + Path.DirectorySeparatorChar + "zillow_listing_1.html");
            // act 
            var listingDetail = _listingParser.Parse(html);
            // assert 
            Assert.NotNull(listingDetail);
            Assert.Equal(expectedListingDetail.ListingPrice, listingDetail.ListingPrice);
            Assert.Equal(expectedListingDetail.NumOfBedrooms, listingDetail.NumOfBedrooms);
            Assert.Equal(expectedListingDetail.NumOfBathrooms, listingDetail.NumOfBathrooms);
            Assert.Equal(expectedListingDetail.NumOfStories, listingDetail.NumOfStories);
            Assert.Equal(expectedListingDetail.NumOfParkingSpaces, listingDetail.NumOfParkingSpaces);
            Assert.Equal(expectedListingDetail.LotSizeInSqrtFt, listingDetail.LotSizeInSqrtFt);
            Assert.Equal(expectedListingDetail.NumOfGarageSpaces, listingDetail.NumOfGarageSpaces);
            Assert.Equal(expectedListingDetail.HomeType, listingDetail.HomeType);
            Assert.Equal(expectedListingDetail.PropertyCondition, listingDetail.PropertyCondition);
            Assert.Equal(expectedListingDetail.YearBuilt, listingDetail.YearBuilt);
            Assert.Equal(expectedListingDetail.HasHOA, listingDetail.HasHOA);
        }

        [Fact]
        public void GetListingDetail_ThrowErrorOnNullArgument()
        {
            // arrange
        }
    }
}