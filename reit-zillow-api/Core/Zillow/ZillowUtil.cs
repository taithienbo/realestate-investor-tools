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
            return $@"{BaseUrl}/{zipCode}";
        }
    }
}
