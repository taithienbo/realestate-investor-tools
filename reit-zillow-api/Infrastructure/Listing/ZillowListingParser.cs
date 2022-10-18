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
                element => element.InnerText.Contains("Bedrooms: "));
                listingDetail.NumOfBathrooms = ExtractNumFromFirstNode(spanElementsUnderFactsAndFeatures,
                element => element.InnerText.Contains("Bathrooms: "));
                listingDetail.NumOfStories = ExtractNumFromFirstNode(spanElementsUnderFactsAndFeatures,
                    (element => element.InnerText.Contains("Stories: ")));
                listingDetail.NumOfParkingSpaces = ExtractNumFromFirstNode(spanElementsUnderFactsAndFeatures,
                    element => element.InnerText.Contains("Total spaces: "));
                listingDetail.LotSizeInSqrtFt = ExtractNumFromFirstNode(spanElementsUnderFactsAndFeatures,
                    element => element.InnerText.Contains("Lot size: "));
                listingDetail.NumOfGarageSpaces = ExtractNumFromFirstNode(spanElementsUnderFactsAndFeatures,
                    element => element.InnerText.Contains("Garage spaces: "));
                listingDetail.HomeType = ExtractHomeTypeFromText(ExtractTextFromFirstNode(spanElementsUnderFactsAndFeatures,
                    element => element.InnerText.Contains("Home type: ")));
                listingDetail.PropertyCondition = ExtractPropertyConditionFromText(ExtractTextFromFirstNode(spanElementsUnderFactsAndFeatures,
                    element => element.InnerText.Contains("Property condition: ")));
                listingDetail.YearBuilt = ExtractNumFromFirstNode(spanElementsUnderFactsAndFeatures,
                    element => element.InnerText.Contains("Year built: "));
                listingDetail.HasHOA = ParseHasHOA(spanElementsUnderFactsAndFeatures);
            }
        }

        private string? ExtractPropertyConditionFromText(string? text)
        {
            if (text == null)
            {
                return null;
            }
            if (text.Contains("Turnkey"))
            {
                return "Turnkey";
            }
            if (text.Contains("Fixer"))
            {
                return "Fixer";
            }
            return null;
        }

        private string? ExtractHomeTypeFromText(string? text)
        {
            if (text == null)
            {
                return null;
            }
            if (text.Contains("SingleFamily"))
            {
                return "SingleFamily";
            }
            return null;
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
