using Core.Dto;
using Core.Listing;
using Core.Zillow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Analyzer
{
    public class MultiplePropertyAnalyzer : IMultiplePropertyAnalyzer
    {
        private IPropertyAnalyzer _propertyAnalyzer;
        private IZillowClient _zillowClient;
        private IHouseSearchParser _houseSearchParser;

        public MultiplePropertyAnalyzer(IPropertyAnalyzer propertyAnalyzer, IZillowClient zillowClient,
            IHouseSearchParser houseSearchParser)
        {
            _propertyAnalyzer = propertyAnalyzer;
            _zillowClient = zillowClient;
            _houseSearchParser = houseSearchParser;
        }

        public async Task<IDictionary<string, PropertyAnalysisDetail>> AnalyzeProperties(int zipCode)
        {
            IDictionary<string, PropertyAnalysisDetail> propertiesAnalysis = new Dictionary<string, PropertyAnalysisDetail>();

            var addressesHtml = await _zillowClient.SearchListingsByZipCode(zipCode);
            IList<string> addresses = _houseSearchParser.ParseListingAddresses(addressesHtml);
            foreach (var address in addresses)
            {
                var analysisResult = await _propertyAnalyzer.AnalyzeProperty(address);
                if (analysisResult != null)
                {
                    propertiesAnalysis[address] = analysisResult;
                }
            }
            return await Task.FromResult(propertiesAnalysis);
        }
    }
}
