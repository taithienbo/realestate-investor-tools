using Core.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace reit_zillow_api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OptionsController : ControllerBase
    {
        private readonly IOptions<AppOptions> _appConfigs;

        public OptionsController(IOptions<AppOptions> appConfigs)
        {
            _appConfigs = appConfigs;

        }

        [HttpGet]
        public AppOptions GetConfigs()
        {
            return _appConfigs.Value;
        }
    }
}
