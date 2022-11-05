using Core.Dto;
using Core.Listing;
using Core.Zillow;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;

namespace reit_zillow_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ListingController : ControllerBase
    {
        private readonly IZillowClient _zillowClient;

        private readonly ILogger<ListingController> _logger;
        private readonly IListingParser _listingParser;

        public ListingController(ILogger<ListingController> logger,
            IZillowClient zillowClient,
            IListingParser listingParser)
        {
            _logger = logger;
            _zillowClient = zillowClient;
            _listingParser = listingParser;
        }

        [HttpGet]
        [Route("GetListingInfo")]
        public async Task<ListingDetail> GetListingInfo(string address)
        {
            if (address == null)
            {
                throw new ArgumentNullException("Address cannot be null");
            }
            var html = await _zillowClient.GetListingHtmlPage(address);
            return _listingParser.Parse(html);
        }
    }
}