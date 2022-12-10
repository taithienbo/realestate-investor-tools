using Core.Analyzer;
using Core.Dto;
using Moq;
using reit_zillow_api.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Tests
{
    public class PropertiesAnalyzerControllerTests
    {
        private PropertiesAnalyzerController _controller;
        private Mock<IMultiplePropertyAnalyzer> _propertiesAnalyzerMock;

        public PropertiesAnalyzerControllerTests()
        {
            _propertiesAnalyzerMock = new Mock<IMultiplePropertyAnalyzer>();
            _controller = new PropertiesAnalyzerController(_propertiesAnalyzerMock.Object);
        }

        [Fact]
        public void AnalyzePropertiesByZipCode()
        {
            // arrange 
            int zipCode = 12345;
            string address = "12345";
            var expectedResult = new Dictionary<string, PropertyAnalysisDetail>();
            expectedResult.Add(address, new PropertyAnalysisDetail());
            _propertiesAnalyzerMock.Setup(analyzer => analyzer.AnalyzeProperties(zipCode)).ReturnsAsync(expectedResult);
            // act 
            var actualResult = _controller.Analyze(zipCode).Result;
            // assert 
            Assert.NotNull(actualResult);
            Assert.Equal(expectedResult, actualResult);

        }

        [Fact]
        public void AnalyzePropertyByAddress()
        {
            // arrange. 
            var expectedAnalysisDetail = new PropertyAnalysisDetail();
            string address = "123";
            _propertiesAnalyzerMock.Setup(analyzer => analyzer.Analyze(It.Is<string>(value => value.Equals(address)))).ReturnsAsync(expectedAnalysisDetail);

            // act 
            var propertyAnalysisDetail = _controller.Analyze(address).Result;
            // assert 
            Assert.NotNull(propertyAnalysisDetail);
            Assert.Equal(expectedAnalysisDetail, propertyAnalysisDetail);

        }
    }
}
