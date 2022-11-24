using Core.Analyzer;
using Core.Constants;
using Core.Dto;
using Core.Listing;
using Core.Zillow;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Core.Tests.Analysis
{
    public class MultiplePropertyAnalyzerTest
    {
        private IMultiplePropertyAnalyzer _multiplePropertyAnalyzer;
        private Mock<IZillowClient> _mockZillowClient;
        private Mock<IHouseSearchParser> _mockHouseSearchParser;
        private Mock<IPropertyAnalyzer> _mockPropertyAnalyzer;

        public MultiplePropertyAnalyzerTest()
        {
            _mockZillowClient = new Mock<IZillowClient>();
            _mockHouseSearchParser = new Mock<IHouseSearchParser>();
            _mockPropertyAnalyzer = new Mock<IPropertyAnalyzer>();
            _multiplePropertyAnalyzer = new MultiplePropertyAnalyzer(_mockPropertyAnalyzer.Object, _mockZillowClient.Object, _mockHouseSearchParser.Object);
        }

        [Fact]
        public void AnalyzeProperties()
        {
            // arrange 
            const int ZipCode = 92805;
            string address = $"123 Test Ave, Santa Ana, CA {ZipCode}";
            string addressHtml = @$"""address"":""${address}""";
            PropertyAnalysisDetail expectedAnalysisDetail = FakePropertyAnalysisDetail();
            IDictionary<string, PropertyAnalysisDetail> expectedResult = new Dictionary<string, PropertyAnalysisDetail>();
            expectedResult.Add(address, expectedAnalysisDetail);
            IList<string> listingAddresses = new List<string>() { address };

            _mockZillowClient.Setup(zillowClient => zillowClient.SearchListingsByZipCode(It.IsAny<int>())).ReturnsAsync(addressHtml);
            _mockHouseSearchParser.Setup(parser => parser.ParseListingAddresses(It.Is<string>(value => value.Equals(addressHtml)))).Returns(listingAddresses);
            _mockPropertyAnalyzer.Setup(analyzer => analyzer.AnalyzeProperty(It.Is<string>(value => value.Equals(address)))).ReturnsAsync(expectedAnalysisDetail);

            // act 
            var propertiesAnalysis = _multiplePropertyAnalyzer.AnalyzeProperties(ZipCode)?.Result;
            Assert.NotNull(propertiesAnalysis);
            Assert.True(propertiesAnalysis!.ContainsKey(address));
            Assert.Equal(expectedAnalysisDetail, propertiesAnalysis[address]);
        }

        private PropertyAnalysisDetail FakePropertyAnalysisDetail()
        {
            Random random = new Random();
            PropertyAnalysisDetail result = new PropertyAnalysisDetail();
            result.Incomes = FakeIncomes();
            result.Expenses = FakeExpenses();
            result.InterestRate = random.NextDouble();
            result.NetOperatingIncome = random.NextDouble();
            result.CapRate = random.NextDouble();
            result.DebtServiceCoverageRatio = random.NextDouble();
            result.CashOnCashReturn = random.NextDouble();
            result.CashFlow = random.NextDouble();

            return result;
        }

        private IDictionary<string, double> FakeIncomes()
        {
            return new Dictionary<string, double>()
            {
                { nameof(CommonIncomeType.Rental), 3000}
            };
        }

        private IDictionary<string, double> FakeExpenses()
        {
            return new Dictionary<string, double>()
            {
                { nameof(CommonExpenseType.Mortgage), 2000}
            };
        }
    }
}
