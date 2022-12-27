using Core.Dto;
using Core.Listing;
using Core.Zillow;
using Microsoft.AspNetCore.Mvc;

namespace reit_zillow_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ListingController : ControllerBase
    {
        private readonly IListingService _listingService;

        public ListingController(IListingService listingService)
        {
            _listingService = listingService;
        }

        [HttpGet]
        [Route("GetListingInfo")]
        public async Task<ListingDetail> GetListingInfo(string address)
        {
            if (address == null)
            {
                throw new ArgumentNullException("Address cannot be null");
            }
            return await _listingService.GetListingDetail(address);
        }
    }
}