using Core.Listing;
using Core.Zillow;


namespace Infrastructure.Listing
{
    public class HouseSearchService : IHouseSearchService
    {
        private IHouseSearchParser _houseSearchParser;
        private HttpClient _httpClient;

        public HouseSearchService(IHttpClientFactory httpClientFactory, IHouseSearchParser houseSearchParser)
        {
            _httpClient = httpClientFactory.CreateClient("Zillow");
            _houseSearchParser = houseSearchParser;
        }

        public async Task<IList<string>> SearchListings(int zipCode)
        {
            string listingsHtml = await _httpClient.GetStringAsync(ZillowUtil.BuildSearchListingsUrl(zipCode));
            return _houseSearchParser.ParseListingAddresses(listingsHtml);
        }
    }
}
