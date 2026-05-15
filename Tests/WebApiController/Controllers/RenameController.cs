using KY.Generator;
using Microsoft.AspNetCore.Mvc;
using WebApiController.Models;

namespace WebApiController.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [GenerateAngularService("ClientApp/src/app/rename/services", "ClientApp/src/app/rename/models")]
    public class RenameController : ControllerBase
    {
        [HttpGet]
        [GenerateMethod(Replace = "Dto", With = "Model")]
        public RenameDto RenameDtoToModel(int id)
        {
            return new RenameDto();
        }

        [HttpGet]
        [GenerateMethod(Replace = "Dummy")]
        public DummyData RemoveDummy(int id)
        {
            return new DummyData();
        }
    }
}
