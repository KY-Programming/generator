using KY.Generator;
using Microsoft.AspNetCore.Mvc;
using WebApiController.Models;

namespace WebApiController.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [GenerateAngularService("ClientApp/src/app/keep-casing/services", "ClientApp/src/app/keep-casing/models")]
    public class KeepCasingController : ControllerBase
    {
        [HttpGet]
        public KeepCasingModel Get()
        {
            return new KeepCasingModel();
        }

        [HttpPost]
        public void Post(KeepCasingModel model)
        {
        }
    }
}
