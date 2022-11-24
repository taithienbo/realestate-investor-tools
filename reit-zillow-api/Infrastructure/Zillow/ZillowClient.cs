using Core.Zillow;

namespace Infrastructure.Zillow
{
    public class ZillowClient : IZillowClient
    {
        private HttpClient _httpClient;

        public ZillowClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Task<string> GetListingHtmlPage(string address)
        {
            return _httpClient.GetStringAsync(ZillowUtil.BuildListingUrl(address));
        }

        public Task<string> GetPriceMyRentalHtmlPage(string address)
        {
            return _httpClient.GetStringAsync(ZillowUtil.BuildPriceMyRentalUrl(address));
        }

        public Task<string> SearchListingsByZipCode(int zipCode)
        {
            return _httpClient.GetStringAsync(ZillowUtil.BuildSearchListingsUrl(zipCode));
        }
    }
}
