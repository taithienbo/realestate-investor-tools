using Core.Income;
using Infrastructure.Income;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Tests.Income
{
    public class ZillowPriceMyRentalParserTest
    {
        private IPriceMyRentalParser _parser;

        public ZillowPriceMyRentalParserTest()
        {
            _parser = new ZillowPriceMyRentalParser();
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
