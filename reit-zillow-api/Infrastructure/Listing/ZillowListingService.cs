using Core.Dto;
using Core.Listing;
using Core.Zillow;

namespace Infrastructure.Listing
{
    public class ZillowListingService : IListingService
    {
        private readonly IZillowListingParser _zillowListingParser;
        private readonly HttpClient _httpClient;

        public ZillowListingService(
            IHttpClientFactory httpClientFactory,
            IZillowListingParser zillowListingParser)
        {
            _httpClient = httpClientFactory.CreateClient("Zillow");
            _zillowListingParser = zillowListingParser;
        }

        public async Task<ListingDetail> GetListingDetail(string address)
        {
            var listingDetailHtml = await GetListingHtmlPage(address);
            return _zillowListingParser.Parse(listingDetailHtml);
        }

        private async Task<string> GetListingHtmlPage(string address)
        {

            return await _httpClient.GetStringAsync(ZillowUtil.BuildListingUrl(address));
        }
    }
}
