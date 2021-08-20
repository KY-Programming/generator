using KY.Generator;
using Microsoft.AspNetCore.Mvc;
using WebApiController.Models;

namespace WebApiController.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [GenerateAngularService("ClientApp/src/app/post/services", "ClientApp/src/app/post/models")]
    public class PostController : ControllerBase
    {
        [HttpPost("[action]")]
        public void PostWithoutParameter()
        { }

        [HttpPost("[action]")]
        public void PostWithOneParameter(string test)
        { }

        [HttpPost("[action]")]
        public void PostWithTwoParameter(string text, int count)
        { }

        [HttpPost("[action]")]
        public void PostWithBodyParameter([FromBody] PostModel model)
        { }

        [HttpPost("[action]")]
        public void PostWithValueAndBodyParameter(int id, [FromBody] PostModel model)
        { }

        [HttpPost("[action]")]
        public void PostWithValueAndBodyParameterFlipped([FromBody] PostModel model, int id)
        { }
    }
}
