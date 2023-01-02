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


        public Task<string> SearchListingsByZipCode(int zipCode)
        {
            return _httpClient.GetStringAsync(ZillowUtil.BuildSearchListingsUrl(zipCode));
        }
    }
}
