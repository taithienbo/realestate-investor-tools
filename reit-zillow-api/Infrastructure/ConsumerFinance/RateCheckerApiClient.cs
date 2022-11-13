using Core.ConsumerFinance;
using Microsoft.AspNetCore.WebUtilities;
using System.Net.Http.Json;

namespace Infrastructure.ConsumerFinance
{
    public class RateCheckerApiClient : IRateCheckerApiClient
    {
        private HttpClient _httpClient;

        public RateCheckerApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<RateCheckerResponse?> CheckRate(RateCheckerRequestInfo request)
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
            var response = await _httpClient.GetFromJsonAsync<RateCheckerResponse>(uri);
            return response;
        }
    }
}
