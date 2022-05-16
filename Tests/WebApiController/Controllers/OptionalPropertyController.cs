using KY.Generator;
using Microsoft.AspNetCore.Mvc;
using WebApiController.Models;

namespace WebApiController.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [GenerateAngularService("ClientApp/src/app/date/services", "ClientApp/src/app/date/models")]
    public class OptionalPropertyController : ControllerBase
    {
        [HttpGet("[action]")]
        public OptionalPropertiesModel GetOptionalPropertiesModel()
        {
            return new OptionalPropertiesModel();
        }
    }
}
