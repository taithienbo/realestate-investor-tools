using Core.Dto;
using Core.Income;
using Core.Zillow;
using Infrastructure.Income;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Tests.Income
{
    public class PriceRentalServiceTests
    {
        private readonly IPriceRentalService _priceRentalService;
        private readonly Mock<IPriceRentalParser> _mockRentalParser;
        private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;
        private readonly Mock<HttpMessageHandler> _mockMessageHandler;

        public PriceRentalServiceTests()
        {
            _mockRentalParser = new Mock<IPriceRentalParser>();

            _mockMessageHandler = new Mock<HttpMessageHandler>();
            _mockHttpClientFactory = new Mock<IHttpClientFactory>();
            _mockHttpClientFactory.Setup(factory => factory.CreateClient(It.IsAny<string>())).Returns(new HttpClient(_mockMessageHandler.Object));

            _priceRentalService = new PriceRentalService(_mockHttpClientFactory.Object, _mockRentalParser.Object);
        }

        [Fact]
        public void PriceMyRental()
        {
            // arrange 
            const double ExpectedRentalPrice = 3000;
            const string Address = "123 Test St, Santa Ana, CA";
            const string TestPriceRentalHtmlPage = "Test content. Does not matter";

            _mockMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(TestPriceRentalHtmlPage)
                });

            _mockRentalParser.Setup(priceRentalParser => priceRentalParser.Parse(It.Is<string>(html => html == TestPriceRentalHtmlPage))).Returns(new PriceRentalDetail()
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
