using KY.Generator;
using Microsoft.AspNetCore.Mvc;

namespace WebApiController.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [GenerateAngularService("ClientApp/src/app/duplicate-name/services", "ClientApp/src/app/duplicate-name/models")]
    [GenerateOption(GenerateOption.SkipHeader)]
    public class DuplicateNameController : ControllerBase
    {
        [HttpGet("{id}")]
        public void TestA(int id)
        { }

        [HttpGet("{id}/{variantA}")]
        public void TestA(int id, string variantA)
        { }

        [HttpGet("{id}/{variantA}/{variantB}")]
        public void TestA(int id, string variantA, string variantB)
        { }

        [HttpGet("[action]")]
        public string TestB(int id)
        {
            return "test1";
        }

        [HttpGet]
        public string TestB(string id)
        {
            return "test1";
        }
    }
}
