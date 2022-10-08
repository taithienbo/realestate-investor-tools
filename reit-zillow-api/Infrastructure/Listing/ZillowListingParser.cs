using Core.Dto;
using Core.Listing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace Infrastructure.Listing
{
    public class ZillowListingParser : IListingParser
    {
        public ListingDetail Parse(string html)
        {
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            var listingDetail = new ListingDetail();
            UpdateListingDetailWithFacts(htmlDoc, listingDetail);
            return listingDetail;
        }

        private void UpdateListingDetailWithFacts(HtmlDocument htmlDoc, ListingDetail listingDetail)
        {
            listingDetail.ListingPrice = ParseListingPrice(htmlDoc);
            var spanElementsUnderFactsAndFeatures = htmlDoc.DocumentNode.SelectNodes("//span");

            if (spanElementsUnderFactsAndFeatures != null && spanElementsUnderFactsAndFeatures.Count > 0)
            {
                listingDetail.NumOfBedrooms = ExtractNumOfBedRooms(spanElementsUnderFactsAndFeatures);
                listingDetail.NumOfBathrooms = ExtractNumOfBathRooms(spanElementsUnderFactsAndFeatures);
            }
        }

        private int ExtractNumOfBathRooms(HtmlNodeCollection spanElementsUnderFactsAndFeatures)
        {
            var numbOfBathsElement = spanElementsUnderFactsAndFeatures.Where(element => element.InnerHtml.Contains("Bathrooms: "));
            var data = Regex.Match(numbOfBathsElement.First().InnerHtml, @"\d+").Value;

            return int.Parse(data);
        }

        private int ExtractNumOfBedRooms(HtmlNodeCollection spanElementsUnderFactsAndFeatures)
        {
            var numOfBedroomsElement = spanElementsUnderFactsAndFeatures.Where(element => element.InnerHtml.Contains("Bedrooms: "));
            var data = Regex.Match(numOfBedroomsElement.First().InnerHtml, @"\d+").Value;

            return int.Parse(data);
        }

        private decimal ParseListingPrice(HtmlDocument htmlDoc)
        {
            var listingPriceElement = htmlDoc.DocumentNode.SelectSingleNode("//span[@data-testid=\"price\"]/span[1]");
            if (listingPriceElement != null)
            {
                var listingPriceText = listingPriceElement.InnerHtml;
                return decimal.Parse(listingPriceText.Replace("$", ""));
            }
            return 0;
        }
    }
}
