using Infrastructure.Util;

namespace Infrastructure.Tests.Util
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
    }
}
