using Core.Analyzer;
using Core.Dto;
using Microsoft.AspNetCore.Mvc;

namespace reit_zillow_api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PropertiesAnalyzerController : ControllerBase
    {
        private readonly IPropertiesAnalyzer _propertiesAnalyzer;

        public PropertiesAnalyzerController(IPropertiesAnalyzer propertiesAnalyzer)
        {
            this._propertiesAnalyzer = propertiesAnalyzer;
        }

        [HttpGet]
        [Route("status")]
        public string Status()
        {
            return "OK";
        }

        [HttpGet]
        [Route("zip")]
        public async Task<IDictionary<string, PropertyAnalysisDetail>> Analyze([FromQuery] int zipCode)
        {
            return await _propertiesAnalyzer.AnalyzeProperties(zipCode);
        }

        [HttpGet]
        [Route("address")]
        public async Task<PropertyAnalysisDetail?> Analyze([FromQuery] string address)
        {
            return await _propertiesAnalyzer.Analyze(address);
        }
    }
}
