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
            string expectedUrl = $"https://www.zillow.com/homes/{zipCode}";
            // act 
            string url = ZillowUtil.BuildSearchListingsUrl(zipCode);
            // assert 
            Assert.NotNull(url);
            Assert.Equal(expectedUrl, url);
        }
    }
}
