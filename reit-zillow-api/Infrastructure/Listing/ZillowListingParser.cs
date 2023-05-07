using Core.Dto;
using Core.Listing;
using Core.Zillow;
using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace Infrastructure.Listing
{
    public class ZillowListingParser : IZillowListingParser
    {
        public ListingDetail Parse(string html)
        {
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            var listingDetail = new ListingDetail();
            UpdateListingDetailWithFacts(htmlDoc, listingDetail);

            UpdateListingDetailWithImageURL(htmlDoc, listingDetail);

            return listingDetail;
        }

        private void UpdateListingDetailWithImageURL(HtmlDocument htmlDoc, ListingDetail listingDetail)
        {
            var imageURLElement = htmlDoc.DocumentNode.SelectSingleNode("//picture/img[1]");
            if (imageURLElement != null)
            {
                var srcAttribute = imageURLElement.Attributes.Where(attribute => attribute.Name.Equals("src"));
                if (srcAttribute != null && srcAttribute.Any())
                {
                    listingDetail.ImageURL = srcAttribute.First().Value;
                }
            }
            // //*[@id="home-details-content"]/div/div/div[1]/div[2]/div[1]/div/div/div/div[1]/div/ul/li[1]/figure/button/picture/img
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
                listingDetail.NumOfLevels = Find(spanElementsUnderFactsAndFeatures, "Levels");
                listingDetail.NumOfParkingSpaces = ExtractNumFromFirstNode(spanElementsUnderFactsAndFeatures,
                    element => element.InnerText.Contains("Total spaces: "));
                listingDetail.LotSize = Find(spanElementsUnderFactsAndFeatures, "Lot size");
                listingDetail.NumOfGarageSpaces = ExtractNumFromFirstNode(spanElementsUnderFactsAndFeatures,
                    element => element.InnerText.Contains("Garage spaces: "));
                listingDetail.HomeType = Find(spanElementsUnderFactsAndFeatures, "Home type");
                listingDetail.PropertyCondition = ExtractPropertyConditionFromText(Find(spanElementsUnderFactsAndFeatures,
                    "Property condition"));
                listingDetail.YearBuilt = ExtractNumFromFirstNode(spanElementsUnderFactsAndFeatures,
                    element => element.InnerText.Contains("Year built: "));
                listingDetail.HasHOA = ParseHasHOA(spanElementsUnderFactsAndFeatures);
                listingDetail.HoaFee = ExtractNumFromFirstNode(spanElementsUnderFactsAndFeatures, element => element.InnerText.Contains("HOA fee: "));
            }
        }

        private HtmlNode? FindFirst(HtmlNodeCollection nodeCollection, Func<HtmlNode, bool> predicate)
        {
            return nodeCollection.Where(predicate).FirstOrDefault();
        }

        private string? ExtractValue(HtmlNode node, string keyword)
        {
            if (node.InnerHtml != null)
            {
                return Regex.Replace(node.InnerHtml!, @"[^\w\s\.@,]", "",
                                RegexOptions.None, TimeSpan.FromSeconds(1.5)).Replace(keyword, "").Trim();
            }

            return null;
        }

        private string? Find(HtmlNodeCollection spanElementsUnderFactsAndFeatures, string keyword)
        {
            var htmlNode = FindFirst(spanElementsUnderFactsAndFeatures, element => element.InnerHtml.Contains(keyword));
            if (htmlNode != null)
                return ExtractValue(htmlNode, keyword);
            return null;
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


        private bool ParseHasHOA(HtmlNodeCollection spanElementsUnderFactsAndFeatures)
        {
            var hoaInfo = Find(spanElementsUnderFactsAndFeatures, "Has HOA");
            return hoaInfo != null && hoaInfo.Equals("Yes");
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


        private double ParseListingPrice(HtmlDocument htmlDoc)
        {
            var listingPriceElement = htmlDoc.DocumentNode.SelectSingleNode("//span[@data-testid=\"price\"]/span[1]");
            if (listingPriceElement != null)
            {
                var listingPriceText = listingPriceElement.InnerHtml;
                return double.Parse(listingPriceText.Replace("$", ""));
            }
            return 0;
        }

        public ListingDetail Get(string address)
        {
            throw new NotImplementedException();
        }
    }
}
