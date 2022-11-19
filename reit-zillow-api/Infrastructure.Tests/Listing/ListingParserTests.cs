using Core.Constants;
using Core.Dto;
using Core.Listing;
using Infrastructure.Listing;

namespace Infrastructure.Tests.Listing
{
    public class ListingParserTests
    {
        private readonly IListingParser _listingParser;

        public ListingParserTests()
        {
            _listingParser = new ListingParser();
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
                NumOfLevels = "One",
                NumOfParkingSpaces = 2,
                LotSize = "8,849 sqft",
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
                LotSize = "4,796 sqft",
                NumOfGarageSpaces = 2,
                NumOfLevels = "One",
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
        public void GetListingDetail_Test3_Condo()
        {
            // arrange 
            string html = File.ReadAllText("TestFiles" + Path.DirectorySeparatorChar + "zillow_listing_condo.html");
            // act 
            var listingDetail = _listingParser.Parse(html);
            var expectedListingDetail = new ListingDetail()
            {
                ListingPrice = 380000,
                HomeType = HomeType.Condo,
                NumOfBathrooms = 1,
                NumOfBedrooms = 1,
                NumOfLevels = "One",
                LotSize = "7 Acres",
                YearBuilt = 1965,
                HasHOA = true
            };
            AssertCorrectListingDetail(expectedListingDetail, listingDetail);
        }


        private void AssertCorrectListingDetail(ListingDetail expectedListingDetail, ListingDetail actualListingDetail)
        {
            // assert 
            Assert.NotNull(actualListingDetail);
            Assert.Equal(expectedListingDetail.ListingPrice, actualListingDetail.ListingPrice);
            Assert.Equal(expectedListingDetail.NumOfBedrooms, actualListingDetail.NumOfBedrooms);
            Assert.Equal(expectedListingDetail.NumOfBathrooms, actualListingDetail.NumOfBathrooms);
            Assert.Equal(expectedListingDetail.NumOfStories, actualListingDetail.NumOfStories);
            Assert.Equal(expectedListingDetail.NumOfLevels, actualListingDetail.NumOfLevels);
            Assert.Equal(expectedListingDetail.NumOfParkingSpaces, actualListingDetail.NumOfParkingSpaces);
            Assert.Equal(expectedListingDetail.LotSize, actualListingDetail.LotSize);
            Assert.Equal(expectedListingDetail.NumOfGarageSpaces, actualListingDetail.NumOfGarageSpaces);
            Assert.Equal(expectedListingDetail.HomeType, actualListingDetail.HomeType);
            Assert.Equal(expectedListingDetail.PropertyCondition, actualListingDetail.PropertyCondition);
            Assert.Equal(expectedListingDetail.YearBuilt, actualListingDetail.YearBuilt);
            Assert.Equal(expectedListingDetail.HasHOA, actualListingDetail.HasHOA);
        }
    }
}