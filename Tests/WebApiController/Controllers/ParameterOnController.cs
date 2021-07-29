using KY.Generator;
using Microsoft.AspNetCore.Mvc;

namespace WebApiController.Controllers
{
    [Route("[controller]/{id}")]
    [GenerateAngularService("ClientApp/src/app/parameter-on-controller/services", "ClientApp/src/app/parameter-on-controller/models")]
    [GenerateOption(GenerateOption.SkipHeader)]
    public class ParameterOnController : ControllerBase
    {
        [HttpGet("[action]")]
        public void Get(string test, string id)
        { }
    }
}
