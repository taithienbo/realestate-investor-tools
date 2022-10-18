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
        public async Task<ListingDetail> GetListingInfo(string address)
        {
            if (address == null)
            {
                throw new ArgumentNullException("Address cannot be null");
            }
            var html = await _zillowClient.GetHtml(address);
            return _listingParser.Parse(html);
        }
    }
}