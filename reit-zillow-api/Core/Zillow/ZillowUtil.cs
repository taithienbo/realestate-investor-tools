using System.Text.Json;
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

            Func<IDictionary<string, string>> buildSort = () =>
            {
                return new Dictionary<string, string>() { { "value", "globalrelevanceex" } };
            };

            Func<IDictionary<string, bool>> buildAh = () =>
            {
                return new Dictionary<string, bool>() { { "value", true } };
            };

            Func<IDictionary<string, bool>> buildLand = () =>
            {
                return new Dictionary<string, bool>() { { "value", false } };
            };

            Func<IDictionary<string, bool>> buildApa = () =>
            {
                return new Dictionary<string, bool>() { { "value", false } };
            };

            Func<IDictionary<string, bool>> buildApco = () =>
            {
                return new Dictionary<string, bool>() { { "value", false } };
            };

            Func<IDictionary<string, bool>> buildCon = () =>
            {
                return new Dictionary<string, bool>() { { "value", false } };
            };

            Func<IDictionary<string, bool>> buildManu = () =>
            {
                return new Dictionary<string, bool>() { { "value", false } };
            };

            Func<IDictionary<string, bool>> buildTow = () =>
            {
                return new Dictionary<string, bool>() { { "value", false } };
            };


            Func<IDictionary<string, object>> buildFilterState = () =>
            {
                IDictionary<string, object> filterStateDictionary = new Dictionary<string, object>();
                filterStateDictionary.Add("sort", buildSort());
                filterStateDictionary.Add("ah", buildAh());
                filterStateDictionary.Add("land", buildLand());
                filterStateDictionary.Add("apa", buildApa());
                filterStateDictionary.Add("apco", buildApco());
                filterStateDictionary.Add("con", buildCon());
                filterStateDictionary.Add("manu", buildManu());
                filterStateDictionary.Add("tow", buildTow());

                return filterStateDictionary;
            };
            ///
            /// https://www.zillow.com/anaheim-ca-92805/?searchQueryState={"usersSearchTerm":"92805","isMapVisible":false,"filterState":{"sort":{"value":"globalrelevanceex"},"ah":{"value":true},"land":{"value":false},"apa":{"value":false},"apco":{"value":false},"con":{"value":false},"manu":{"value":false},"tow":{"value":false}},"isListVisible":true,"mapZoom":14}
            ///
            var queryParameters = new Dictionary<string, object>();
            queryParameters.Add("usersSearchTerm", zipCode.ToString());
            queryParameters.Add("isMapVisible", false);
            queryParameters.Add("filterState", buildFilterState());
            queryParameters.Add("isListVisible", true);
            queryParameters.Add("mapZoom", 14);



            return $@"{BaseUrl}/{zipCode}?searchQueryState={JsonSerializer.Serialize(queryParameters)}";
        }
    }
}
