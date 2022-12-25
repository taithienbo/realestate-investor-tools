using Core.Zillow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Listing
{
    public class HouseSearchService : IHouseSearchService
    {
        private IZillowClient _zillowClient;
        private IHouseSearchParser _houseSearchParser;

        public HouseSearchService(IZillowClient zillowClient, IHouseSearchParser houseSearchParser)
        {
            _zillowClient = zillowClient;
            _houseSearchParser = houseSearchParser;
        }

        public async Task<IList<string>> SearchListings(int zipCode)
        {
            string listingsHtml = await _zillowClient.SearchListingsByZipCode(zipCode);
            return _houseSearchParser.ParseListingAddresses(listingsHtml);
        }
    }
}
