using Core.Dto;
using Core.Listing;
using Core.Zillow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Listing
{
    public class ZillowListingService : IZillowListingService
    {
        private readonly IZillowClient _zillowClient;
        private readonly IZillowListingParser _zillowListingParser;

        public ZillowListingService(IZillowClient zillowClient, IZillowListingParser zillowListingParser)
        {
            _zillowClient = zillowClient;
            _zillowListingParser = zillowListingParser;
        }

        public async Task<ListingDetail> GetListingDetail(string address)
        {
            var listingDetailHtml = await _zillowClient.GetListingHtmlPage(address);
            return _zillowListingParser.Parse(listingDetailHtml);
        }
    }
}
