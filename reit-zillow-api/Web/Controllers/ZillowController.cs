using Core.Dto;
using Core.Listing;
using Core.Zillow;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;

namespace reit_zillow_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ZillowController : ControllerBase
    {
        private readonly IZillowClient _zillowClient;

        private readonly ILogger<ZillowController> _logger;
        private readonly IListingParser _listingParser;

        public ZillowController(ILogger<ZillowController> logger,
            IZillowClient zillowClient,
            IListingParser listingParser)
        {
            _logger = logger;
            _zillowClient = zillowClient;
            _listingParser = listingParser;
        }

        [HttpGet]
        [Route("GetListingInfo")]
        public async Task<string> GetListingInfo()
        {
            string url = @"https://www.zillow.com/homes/829-N-Lenz-Dr-Anaheim-CA-92805";
            HttpClient client = new HttpClient();
            string httpResponse = await client.GetStringAsync(url);

            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(httpResponse);

            var listingPrice = htmlDoc.DocumentNode.SelectSingleNode("//span[@data-testid=\"price\"]/span[1]");
            var listingPriceString = listingPrice.InnerHtml;
            return listingPriceString;
        }

        public async Task<ListingDetail> GetListingInfo(string address)
        {
            var html = await _zillowClient.GetHtml(address);
            return _listingParser.Parse(html);
        }
    }
}