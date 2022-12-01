using Core.Zillow;
using System.Net.Http;

namespace Infrastructure.Zillow
{
    public class ZillowClient : IZillowClient
    {
        private readonly HttpClient _httpClient;
        public ZillowClient(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("Zillow");
        }

        public Task<string> GetListingHtmlPage(string address)
        {
            var handler = new HttpClientHandler
            {
                UseCookies = false
            };
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
