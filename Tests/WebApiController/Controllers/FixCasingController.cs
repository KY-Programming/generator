using KY.Generator;
using Microsoft.AspNetCore.Mvc;
using WebApiController.Models;

namespace WebApiController.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [GenerateAngularService("ClientApp/src/app/fix-casing/services", "ClientApp/src/app/fix-casing/models")]
    [GenerateOption(GenerateOption.SkipHeader)]
    public class FixCasingController : ControllerBase
    {
        [HttpGet]
        public CasingModel Get()
        {
            return new CasingModel();
        }

        [HttpPost]
        public void Post(CasingModel model)
        {
        }
    }
}