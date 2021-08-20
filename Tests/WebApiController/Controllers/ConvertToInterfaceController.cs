using KY.Generator;
using Microsoft.AspNetCore.Mvc;
using WebApiController.Models;

namespace WebApiController.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [GenerateAngularService("ClientApp/src/app/convert-to-interface/services", "ClientApp/src/app/convert-to-interface/models")]
    [GeneratePreferInterfaces]
    public class ConvertToInterfaceController : ControllerBase
    {
        [HttpGet("[action]")]
        public ConvertMe Get(string subject)
        {
            return new ConvertMe();
        }
    }

    [ApiController]
    [Route("[controller]")]
    [GenerateAngularService("ClientApp/src/app/convert-to-interface/services", "ClientApp/src/app/convert-to-interface/models")]
    [GeneratePreferInterfaces]
    [GenerateWithOptionalProperties]
    public class ConvertToInterfaceOptionalController : ControllerBase
    {
        [HttpGet("[action]")]
        public ConvertMeOptional Get(string subject)
        {
            return new ConvertMeOptional();
        }
    }
}
