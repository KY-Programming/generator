using KY.Generator;
using Microsoft.AspNetCore.Mvc;
using WebApiController.Models;

namespace WebApiController.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    [GenerateAngularService("ClientApp/src/app/warnings/services", "ClientApp/src/app/warnings/models")]
    public class WarningController : ControllerBase
    {
        [HttpGet, HttpPost, HttpPatch, HttpPut, HttpDelete]
        public void WithBody([FromBody] WeatherForecast model)
        { }
    }
}
