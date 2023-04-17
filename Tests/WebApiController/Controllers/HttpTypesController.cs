using KY.Generator;
using Microsoft.AspNetCore.Mvc;
using WebApiController.Models;

namespace WebApiController.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    [GenerateAngularService("ClientApp/src/app/http-types/services", "ClientApp/src/app/http-types/models")]
    public class HttpTypesController : ControllerBase
    {
        [HttpGet]
        public void Get()
        { }

        [HttpPost]
        public void Post()
        { }

        [HttpPatch]
        public void Patch(string test)
        { }

        [HttpPut]
        public void Put(string test)
        { }

        [HttpDelete]
        public void Delete()
        { }
    }
}
