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

        public Task<string> GetHtml(string address)
        {
            return _httpClient.GetStringAsync(BuildUrl(ZillowUtil.NormalizeAddress(address)));
        }

        private string BuildUrl(string address)
        {
            return @$"https://www.zillow.com/homes/{address}";
        }
    }
}
