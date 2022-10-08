using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;

namespace reit_zillow_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ZillowController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<ZillowController> _logger;

        public ZillowController(ILogger<ZillowController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet]
        [Route("GetListingInfo")]
        public async Task<string> GetListingInfo()
        {
            string url = @"https://www.zillow.com/homes/829-N-Lenz-Dr-Anaheim-CA-92805";
            HttpClient client = new HttpClient();
            string httpResponse = await client.GetStringAsync(url);

            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(httpResponse);

            var listingPrice = htmlDoc.DocumentNode.SelectSingleNode("//span[@data-testid=\"price\"]/span[1]");
            var listingPriceString = listingPrice.InnerHtml;
            return listingPriceString;
        }
    }
}