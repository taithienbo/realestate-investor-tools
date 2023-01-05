using Core.ConsumerFinance;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Caching.Memory;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Infrastructure.ConsumerFinance
{
    public class RateCheckerApiClient : IRateCheckerApiClient
    {
        private HttpClient _httpClient;
        private readonly IMemoryCache _memoryCache;
        private readonly MemoryCacheEntryOptions _cacheEntryOptions;

        public RateCheckerApiClient(HttpClient httpClient,
            IMemoryCache memoryCache)
        {
            _httpClient = httpClient;

            _cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromSeconds(10))
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(1));
            _memoryCache = memoryCache;
        }

        public async Task<RateCheckerResponse?> CheckRate(RateCheckerRequestInfo request)
        {
            RateCheckerResponse? response = new RateCheckerResponse();
            string key = JsonSerializer.Serialize(request);
            if (!_memoryCache.TryGetValue(key, out response))
            {
                var queryParameters = new Dictionary<string, string?>()
                {
                    ["loan_amount"] = request.LoanAmount.ToString(),
                    ["state"] = request.State.ToString(),
                    ["loan_type"] = request.LoanType,
                    ["minfico"] = request.MinFico.ToString(),
                    ["maxfico"] = request.MaxFico.ToString(),
                    ["rate_structure"] = request.RateStructure,
                    ["loan_term"] = request.LoanTerm.ToString(),
                    ["price"] = request.Price.ToString()
                };
                var uri = QueryHelpers.AddQueryString("https://www.consumerfinance.gov/oah-api/rates/rate-checker", queryParameters);

                response = await _httpClient.GetFromJsonAsync<RateCheckerResponse>(uri);
                _memoryCache.Set(key, response);
            }

            return response;
        }
    }
}
