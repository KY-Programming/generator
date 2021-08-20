using KY.Generator;
using Microsoft.AspNetCore.Mvc;
using WebApiController.Models;

namespace WebApiController.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [GenerateAngularService("ClientApp/src/app/rename/services", "ClientApp/src/app/rename/models")]
    [GenerateRename("Dto", "Model")]
    [GenerateRename("Dummy")]
    public class RenameController : ControllerBase
    {
        [HttpGet]
        public RenameDto RenameDtoToModel(int id)
        {
            return new RenameDto();
        }

        [HttpGet]
        public DummyData RemoveDummy(int id)
        {
            return new DummyData();
        }
    }
}
