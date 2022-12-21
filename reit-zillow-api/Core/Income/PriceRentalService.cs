using Core.Dto;
using Core.Zillow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Income
{
    public class PriceRentalService : IPriceRentalService
    {
        private readonly IZillowClient _zillowClient;
        private readonly IPriceRentalParser _priceRentalParser;

        public PriceRentalService(IZillowClient zillowClient, IPriceRentalParser priceRentalParser)
        {
            _zillowClient = zillowClient;
            _priceRentalParser = priceRentalParser;
        }

        public async Task<double> PriceMyRental(string address)
        {
            string priceRentalHtmlPage = await _zillowClient.GetPriceMyRentalHtmlPage(address);
            PriceRentalDetail priceRentalDetail = _priceRentalParser.Parse(priceRentalHtmlPage);
            return priceRentalDetail.ZEstimate;
        }
    }
}
