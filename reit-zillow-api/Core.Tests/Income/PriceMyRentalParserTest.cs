using Core.Income;

namespace Infrastructure.Tests.Income
{
    public class PriceMyRentalParserTest
    {
        private IPriceRentalParser _parser;

        public PriceMyRentalParserTest()
        {
            _parser = new PriceRentalParser();
        }

        [Fact]
        public void ParseRentalPrice()
        {
            // arrange 
            string html = File.ReadAllText("TestFiles" + Path.DirectorySeparatorChar + "price_my_rental_test1.html");
            // act 
            var priceMyRentalDetail = _parser.Parse(html);
            // assert 
            Assert.NotNull(priceMyRentalDetail);
            const double expectedLow = 3196;
            Assert.Equal(expectedLow, priceMyRentalDetail.ZEstimateLow, 0);
            const double expectedHigh = 3806;
            Assert.Equal(expectedHigh, priceMyRentalDetail.ZEstimateHigh, 0);
            const double expectedEstimate = 3399;
            Assert.Equal(expectedEstimate, priceMyRentalDetail.ZEstimate, 0);
        }
    }
}
