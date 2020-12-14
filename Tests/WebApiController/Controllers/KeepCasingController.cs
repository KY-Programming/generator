using KY.Generator;
using Microsoft.AspNetCore.Mvc;
using WebApiController.Models;

namespace WebApiController.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [GenerateAngularService("ClientApp/src/app/keep-casing/services", "ClientApp/src/app/keep-casing/models", formatModelNames: Option.No)]
    [GenerateOption(GenerateOption.SkipHeader)]
    public class KeepCasingController : ControllerBase
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