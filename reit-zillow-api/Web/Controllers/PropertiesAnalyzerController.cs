using Core.Analyzer;
using Core.Dto;
using Microsoft.AspNetCore.Mvc;

namespace reit_zillow_api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PropertiesAnalyzerController : ControllerBase
    {
        private readonly IMultiplePropertyAnalyzer _propertiesAnalyzer;

        public PropertiesAnalyzerController(IMultiplePropertyAnalyzer propertiesAnalyzer)
        {
            this._propertiesAnalyzer = propertiesAnalyzer;
        }

        [HttpGet]
        public string Status()
        {
            return "OK";
        }

        [HttpGet("{zipCode}")]
        public async Task<IDictionary<string, PropertyAnalysisDetail>> AnalyzeProperties(int zipCode)
        {
            return await _propertiesAnalyzer.AnalyzeProperties(zipCode);
        }


    }
}
