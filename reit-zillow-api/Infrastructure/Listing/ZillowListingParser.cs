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
                listingDetail.NumOfBedrooms = ExtractNumFromFirstNode(spanElementsUnderFactsAndFeatures,
                element => element.InnerHtml.Contains("Bedrooms: "));
                listingDetail.NumOfBathrooms = ExtractNumFromFirstNode(spanElementsUnderFactsAndFeatures,
                element => element.InnerHtml.Contains("Bathrooms: "));
                listingDetail.NumOfStories = ExtractNumFromFirstNode(spanElementsUnderFactsAndFeatures,
                    (element => element.InnerHtml.Contains("Stories: ")));
                listingDetail.NumOfParkingSpaces = ExtractNumFromFirstNode(spanElementsUnderFactsAndFeatures,
                    element => element.InnerHtml.Contains("Total spaces: "));
                listingDetail.LotSizeInSqrtFt = ExtractNumFromFirstNode(spanElementsUnderFactsAndFeatures,
                    element => element.InnerHtml.Contains("Lot size: "));
                listingDetail.NumOfGarageSpaces = ExtractNumFromFirstNode(spanElementsUnderFactsAndFeatures,
                    element => element.InnerHtml.Contains("Garage spaces: "));
                listingDetail.HomeType = ExtractTextFromFirstNode(spanElementsUnderFactsAndFeatures,
                    element => element.InnerHtml.Contains("Home type: "))?.Replace("Home type: ", "");
                listingDetail.PropertyCondition = ExtractTextFromFirstNode(spanElementsUnderFactsAndFeatures,
                    element => element.InnerHtml.Contains("Property condition: "))?.Replace("Property condition: ", "");
                listingDetail.YearBuilt = ExtractNumFromFirstNode(spanElementsUnderFactsAndFeatures,
                    element => element.InnerHtml.Contains("Year built: "));
                listingDetail.HasHOA = ParseHasHOA(spanElementsUnderFactsAndFeatures);
            }
        }

        private bool ParseHasHOA(HtmlNodeCollection spanElementsUnderFactsAndFeatures)
        {
            bool hasHOA;
            bool.TryParse(ExtractTextFromFirstNode(spanElementsUnderFactsAndFeatures,
                    element => element.InnerHtml.Contains("Has HOA: "))?.Replace("Has HOA: ", ""), out hasHOA);
            return hasHOA;
        }

        private string? ExtractTextFromFirstNode(HtmlNodeCollection nodeCollection, Func<HtmlNode, bool> predicate)
        {
            var filteredNodes = nodeCollection.Where(predicate);
            if (filteredNodes == null || filteredNodes.Count() == 0)
            {
                return null;
            }
            return filteredNodes.First().InnerHtml;
        }

        private int ExtractNumFromFirstNode(HtmlNodeCollection nodeCollection, Func<HtmlNode, bool> predicate)
        {
            var filteredNodes = nodeCollection.Where(predicate);
            if (filteredNodes == null || filteredNodes.Count() == 0)
            {
                return 0;
            }
            var match = Regex.Match(filteredNodes.First().InnerHtml.Replace(",", ""), @"\d+").Value;
            if (match == null)
            {
                return 0;
            }
            return int.Parse(match);

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
