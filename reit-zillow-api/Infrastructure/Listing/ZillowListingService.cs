using Core.Dto;
using Core.Listing;
using Core.Zillow;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Listing
{
    public class ZillowListingService : IListingService
    {
        private readonly IZillowListingParser _zillowListingParser;
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _memoryCache;
        private readonly MemoryCacheEntryOptions _cacheEntryOptions;

        public ZillowListingService(
            IMemoryCache memoryCache,
            IHttpClientFactory httpClientFactory,
            IZillowListingParser zillowListingParser)
        {
            _cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(10))
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(1));
            _memoryCache = memoryCache;

            _httpClient = httpClientFactory.CreateClient("Zillow");

            _zillowListingParser = zillowListingParser;
        }

        public async Task<ListingDetail> GetListingDetail(string address)
        {
            ListingDetail listingDetail;
            if (!_memoryCache.TryGetValue(address, out listingDetail!))
            {
                var listingDetailHtml = await GetListingHtmlPage(address);
                listingDetail = _zillowListingParser.Parse(listingDetailHtml);
                _memoryCache.Set(address, listingDetail, _cacheEntryOptions);

            }
            return listingDetail!;
        }

        private async Task<string> GetListingHtmlPage(string address)
        {
            var url = ZillowUtil.BuildListingUrl(address);
            try
            {
                return await _httpClient.GetStringAsync(url);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get listing page for {address}", ex);
            }
        }
    }
}
