using Core.Listing;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Infrastructure.Listing
{
    public class HouseSearchParser : IHouseSearchParser
    {
        public IList<string> ParseListingAddresses(string html)
        {
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            var innerHtml = htmlDocument.DocumentNode.InnerHtml;
            const string Pattern = @"""address"":""([\d\w\s,]*)";
            var matches = Regex.Matches(innerHtml, Pattern);
            var addresses = new List<string>();

            if (matches != null)
            {
                foreach (Match match in matches)
                {
                    addresses.Add(match.Groups[1].Value);
                }
            }

            return addresses;
        }
    }
}
