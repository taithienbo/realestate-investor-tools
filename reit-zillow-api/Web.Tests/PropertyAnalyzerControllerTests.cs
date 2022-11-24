using Core.Analyzer;
using Core.Constants;
using Core.Dto;
using Core.Expense;
using Core.Income;
using Core.Interest;
using Core.Listing;
using Core.Zillow;
using Moq;
using reit_zillow_api.Controllers;

namespace Web.Tests
{
    public class PropertyAnalyzerControllerTests
    {
        private readonly PropertyAnalyzerController _propertyAnalyzerController;

        private Mock<IPropertyAnalyzer> _propertyAnalyzerMock;

        public PropertyAnalyzerControllerTests()
        {
            _propertyAnalyzerMock = new Mock<IPropertyAnalyzer>();
            _propertyAnalyzerController = new PropertyAnalyzerController(_propertyAnalyzerMock.Object);
        }

        [Fact]
        public void AnalyzeProperty()
        {
            // arrange. 
            var expectedAnalysisDetail = new PropertyAnalysisDetail();
            string address = "123";
            _propertyAnalyzerMock.Setup(analyzer => analyzer.AnalyzeProperty(It.Is<string>(value => value.Equals(address)))).ReturnsAsync(expectedAnalysisDetail);

            // act 
            var propertyAnalysisDetail = _propertyAnalyzerController.Analyze(address).Result;
            // assert 
            Assert.NotNull(propertyAnalysisDetail);
            Assert.Equal(expectedAnalysisDetail, propertyAnalysisDetail);
        }
    }
}
