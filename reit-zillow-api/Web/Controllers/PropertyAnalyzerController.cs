using Core;
using Core.Analyzer;
using Core.Constants;
using Core.Dto;
using Core.Expense;
using Core.Income;
using Core.Interest;
using Core.Listing;
using Core.Zillow;
using Microsoft.AspNetCore.Mvc;

namespace reit_zillow_api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PropertyAnalyzerController : ControllerBase
    {
        private readonly IPropertyAnalyzer _propertyAnalyzer;

        public PropertyAnalyzerController(IPropertyAnalyzer propertyAnalyzer)
        {
            _propertyAnalyzer = propertyAnalyzer;
        }

        [HttpGet]
        public string Status()
        {
            return "OK";
        }

        [HttpGet("{address}")]
        public async Task<PropertyAnalysisDetail?> Analyze(string address)
        {
            return await _propertyAnalyzer.AnalyzeProperty(address);
        }
    }
}
