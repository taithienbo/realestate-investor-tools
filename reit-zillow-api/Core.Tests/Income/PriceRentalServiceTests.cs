using Core.Income;
using Core.Zillow;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Tests.Income
{
    public class PriceRentalServiceTests
    {
        private readonly IPriceRentalService _priceRentalService;
        private readonly Mock<IZillowClient> _mockZillowClient;
        private readonly Mock<IPriceRentalParser> _mockRentalParser;

        public PriceRentalServiceTests()
        {
            _mockRentalParser = new Mock<IPriceRentalParser>();
            _mockZillowClient = new Mock<IZillowClient>();

            _priceRentalService = new PriceRentalService(_mockZillowClient.Object, _mockRentalParser.Object);
        }

        [Fact]
        public void PriceMyRental()
        {
            // arrange 
            const double ExpectedRentalPrice = 3000;
            const string Address = "123 Test St, Santa Ana, CA";
            const string TestPriceRentalHtmlPage = "Test content. Does not matter";
            _mockZillowClient.Setup(zillowClient => zillowClient.GetPriceMyRentalHtmlPage(It.Is<string>(address => address == Address))).ReturnsAsync(TestPriceRentalHtmlPage);
            _mockRentalParser.Setup(priceRentalParser => priceRentalParser.Parse(It.Is<string>(html => html == TestPriceRentalHtmlPage))).Returns(new Dto.PriceRentalDetail()
            {
                ZEstimate = ExpectedRentalPrice
            });

            // act 
            double actualRentalPrice = _priceRentalService.PriceMyRental(Address).Result;
            // assert 
            Assert.Equal(ExpectedRentalPrice, actualRentalPrice, 0);
        }
    }
}
