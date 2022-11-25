using System.Text.RegularExpressions;

namespace Core.Zillow
{
    public class ZillowUtil
    {
        private const string BaseUrl = "https://www.zillow.com/homes";

        public static string NormalizeAddress(string address)
        {
            return Regex.Replace(address.Replace(",", " "), @"\s+", " ").Replace(" ", "-");
        }

        public static string BuildListingUrl(string address)
        {
            return @$"{BaseUrl}/{NormalizeAddress(address)}";
        }

        public static string BuildPriceMyRentalUrl(string address)
        {
            return $@"https://www.zillow.com/rental-manager/price-my-rental/results/{NormalizeAddress(address)}/";
        }

        public static string BuildSearchListingsUrl(int zipCode)
        {

            Func<IDictionary<string, IDictionary<string, string>>> buildSort = () =>
            {
                IDictionary<string, string> sortDictionary = new Dictionary<string, string>();
                sortDictionary.Add("value", "globalrelevanceex");

                return new Dictionary<string, IDictionary<string, string>>() { { "sort", sortDictionary } };
            };

            Func<IDictionary<string, object>> buildFilterState = () =>
            {
                IDictionary<string, object> filterStateDictionary = new Dictionary<string, object>();
                filterStateDictionary.Add("sort", buildSort());
                return filterStateDictionary;
            };
            ///
            /// https://www.zillow.com/anaheim-ca-92805/?searchQueryState={"usersSearchTerm":"92805","isMapVisible":false,"filterState":{"sort":{"value":"globalrelevanceex"},"ah":{"value":true},"land":{"value":false},"apa":{"value":false},"apco":{"value":false},"con":{"value":false},"manu":{"value":false},"tow":{"value":false}},"isListVisible":true,"mapZoom":14}
            ///
            var queryParameters = new Dictionary<string, object>();
            queryParameters.Add("usersSearchTerm", zipCode);
            queryParameters.Add("isMapVisible", false);
            var filterState = buildFilterState(); 
          
        
            return $@"{BaseUrl}/{zipCode}";
        }
    }
}
