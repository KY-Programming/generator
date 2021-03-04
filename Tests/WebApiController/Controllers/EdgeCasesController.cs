using System.Collections.Generic;
using System.Threading;
using KY.Generator;
using Microsoft.AspNetCore.Mvc;

namespace WebApiController.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [GenerateAngularService("ClientApp/src/app/edge-cases/services", "ClientApp/src/app/edge-cases/models")]
    [GenerateOption(GenerateOption.SkipHeader)]
    public class EdgeCasesController : ControllerBase
    {
        [HttpGet]
        public void Get(string subject)
        { }

        [HttpPost]
        public void Post(string subject)
        { }
        
        [HttpGet]
        public List<string> Cancelable(string subject, CancellationToken token)
        {
            return new List<string> { subject };
        }
    }
}