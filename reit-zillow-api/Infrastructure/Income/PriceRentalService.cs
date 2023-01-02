using Core.Dto;
using Core.Income;
using Core.Zillow;


namespace Infrastructure.Income
{
    public class PriceRentalService : IPriceRentalService
    {
        private readonly IPriceRentalParser _priceRentalParser;
        private readonly HttpClient _httpClient;
        public PriceRentalService(
            IHttpClientFactory httpClientFactory,
            IPriceRentalParser priceRentalParser)
        {
            _httpClient = httpClientFactory.CreateClient("Zillow");
            _priceRentalParser = priceRentalParser;
        }

        public async Task<double> PriceMyRental(string address)
        {
            string priceRentalHtmlPage = await _httpClient.GetStringAsync(ZillowUtil.BuildPriceMyRentalUrl(address));
            PriceRentalDetail priceRentalDetail = _priceRentalParser.Parse(priceRentalHtmlPage);
            return priceRentalDetail.ZEstimate;
        }
    }
}
