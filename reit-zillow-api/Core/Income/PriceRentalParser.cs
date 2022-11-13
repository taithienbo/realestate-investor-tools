using Core.Dto;
using System.Text.RegularExpressions;

namespace Core.Income
{
    public class PriceRentalParser : IPriceRentalParser
    {
        public PriceRentalDetail Parse(string html)
        {
            var match = Regex.Match(html, "\"rentZestimate\":(?<price>\\d+),");

            return new PriceRentalDetail()
            {
                ZEstimateLow = ParseZEstimateLow(html),
                ZEstimateHigh = ParseZEstimateHigh(html),
                ZEstimate = ParseZEstimate(html)
            };
        }

        private double ParseEstimate(string html, string keyword)
        {
            var match = Regex.Match(html, $"\"{keyword}\":(?<price>\\d+),");
            double result = 0;
            if (match != null)
            {
                double.TryParse(match.Groups["price"].Value, out result);
            }
            return result;
        }

        private double ParseZEstimate(string html)
        {
            return ParseEstimate(html, "rentZestimate");
        }

        private double ParseZEstimateHigh(string html)
        {
            return ParseEstimate(html, "rentZestimateRangeHigh");
        }


        private double ParseZEstimateLow(string html)
        {
            return ParseEstimate(html, "rentZestimateRangeLow");
        }
    }
}
