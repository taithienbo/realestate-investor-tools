using Core.Listing;
using Infrastructure.Listing;

namespace Infrastructure.Tests.Listing
{
    public class ZillowListingParserTests
    {
        private readonly IListingParser _listingParser;

        public ZillowListingParserTests()
        {
            _listingParser = new ZillowListingParser();
        }

        [Fact]
        public void GetListingDetail_Test1()
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
            AssertCorrectListingDetail(expectedListingDetail, listingDetail);
        }

        [Fact]
        public void GetListingDetail_Test2()
        {
            var expectedListingDetail = new ListingDetail()
            {
                ListingPrice = 885000,
                NumOfBedrooms = 3,
                NumOfBathrooms = 2,
                NumOfStories = 1,
                NumOfParkingSpaces = 4,
                LotSizeInSqrtFt = 4796,
                NumOfGarageSpaces = 2,
                HomeType = "SingleFamily",
                PropertyCondition = "Turnkey",
                YearBuilt = 1971,
                HasHOA = false
            };
            // arrange 
            string html = File.ReadAllText("TestFiles" + Path.DirectorySeparatorChar + "zillow_listing_2.html");
            // act 
            var listingDetail = _listingParser.Parse(html);
            // assert
            AssertCorrectListingDetail(expectedListingDetail, listingDetail);
        }

        [Fact]
        public void GetListingDetail_ThrowErrorOnNullArgument()
        {
            // arrange
        }

        private void AssertCorrectListingDetail(ListingDetail expectedListingDetail, ListingDetail actualListingDetail)
        {
            // assert 
            Assert.NotNull(actualListingDetail);
            Assert.Equal(expectedListingDetail.ListingPrice, actualListingDetail.ListingPrice);
            Assert.Equal(expectedListingDetail.NumOfBedrooms, actualListingDetail.NumOfBedrooms);
            Assert.Equal(expectedListingDetail.NumOfBathrooms, actualListingDetail.NumOfBathrooms);
            Assert.Equal(expectedListingDetail.NumOfStories, actualListingDetail.NumOfStories);
            Assert.Equal(expectedListingDetail.NumOfParkingSpaces, actualListingDetail.NumOfParkingSpaces);
            Assert.Equal(expectedListingDetail.LotSizeInSqrtFt, actualListingDetail.LotSizeInSqrtFt);
            Assert.Equal(expectedListingDetail.NumOfGarageSpaces, actualListingDetail.NumOfGarageSpaces);
            Assert.Equal(expectedListingDetail.HomeType, actualListingDetail.HomeType);
            Assert.Equal(expectedListingDetail.PropertyCondition, actualListingDetail.PropertyCondition);
            Assert.Equal(expectedListingDetail.YearBuilt, actualListingDetail.YearBuilt);
            Assert.Equal(expectedListingDetail.HasHOA, actualListingDetail.HasHOA);
        }
    }
}