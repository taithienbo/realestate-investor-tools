using Microsoft.AspNetCore.Mvc;

namespace reit_zillow_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StatusController : ControllerBase
    {
        [HttpGet]
        public string Status()
        {
            return "App is up";
        }

    }
}
