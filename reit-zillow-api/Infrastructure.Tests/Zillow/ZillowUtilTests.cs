using Core.Zillow;

namespace Infrastructure.Tests.Zillow
{
    public class ZillowUtilTests
    {
        [Fact]
        public void NormalizeAddress()
        {
            string address = "230 E Susanne St, Anaheim, CA 92805";
            string expectedNormalizedAddress = "230-E-Susanne-St-Anaheim-CA-92805";
            Assert.Equal(expectedNormalizedAddress, ZillowUtil.NormalizeAddress(address));
        }

        [Fact]
        public void BuildSearchListingsUrl()
        {
            // arrange 
            int zipCode = 92805;
            string expectedUrl = @"https://www.zillow.com/homes/92805?searchQueryState={""usersSearchTerm"":""92805"",""isMapVisible"":false,""filterState"":{""sort"":{""value"":""globalrelevanceex""},""ah"":{""value"":true},""land"":{""value"":false},""apa"":{""value"":false},""apco"":{""value"":false},""con"":{""value"":false},""manu"":{""value"":false},""tow"":{""value"":false}},""isListVisible"":true,""mapZoom"":14}";
            // act 
            string url = ZillowUtil.BuildSearchListingsUrl(zipCode);
            // assert 
            Assert.NotNull(url);
            Assert.Equal(expectedUrl, url);
        }
    }
}
