using KY.Generator;
using Microsoft.AspNetCore.Mvc;

namespace AspDotNetAnnotations.Controllers
{
    [Route("[controller]/{id}")]
    [GenerateAngularService("ClientApp/src/app/parameter-on-controller/services", "ClientApp/src/app/parameter-on-controller/models")]
    public class ParameterOnController : ControllerBase
    {
        [HttpGet("[action]")]
        public void Get(string test, string id)
        { }
    }
}
