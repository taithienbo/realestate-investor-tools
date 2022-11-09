using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Core.Zillow
{
    public class ZillowUtil
    {
        public static string NormalizeAddress(string address)
        {
            return Regex.Replace(address.Replace(",", " "), @"\s+", " ").Replace(" ", "-");
        }

        public static string BuildListingUrl(string address)
        {
            return @$"https://www.zillow.com/homes/{NormalizeAddress(address)}";
        }

        public static string BuildPriceMyRentalUrl(string address)
        {
            return $@"https://www.zillow.com/rental-manager/price-my-rental/results/{NormalizeAddress(address)}/";
        }
    }
}
