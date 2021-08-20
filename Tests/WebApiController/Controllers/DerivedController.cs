using KY.Generator;
using Microsoft.AspNetCore.Mvc;

namespace WebApiController.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [GenerateAngularService("ClientApp/src/app/derived/services", "ClientApp/src/app/derived/models")]
    public class DerivedController : DerivableController
    {
        [HttpGet("[action]")]
        public void Get()
        {

        }

        public override void Overwritten()
        {
            base.Overwritten();
        }
    }

    public class DerivableController : ControllerBase
    {
        [HttpGet("[action]")]
        public void GetBase()
        {

        }

        [HttpGet("[action]")]
        public virtual void Overwritten()
        {

        }

        [HttpGet("[action]")]
        public virtual void NotOverwritten()
        {

        }

        [NonAction]
        public void NonAction()
        {

        }
    }
}
