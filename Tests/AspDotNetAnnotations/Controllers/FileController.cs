using KY.Generator;
using Microsoft.AspNetCore.Mvc;

namespace AspDotNetAnnotations.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [GenerateAngularService("ClientApp/src/app/file/services", "ClientApp/src/app/file/models")]
    public class FileController : ControllerBase
    {
        [HttpGet("content")]
        public FileContentResult Content()
        {
            return this.File([], "application/zip", "content.zip");
        }

        [HttpGet("stream")]
        public async Task<FileStreamResult> Stream()
        {
            return await Task.FromResult(this.File(new MemoryStream(), "application/zip", "stream.zip"));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
        public IActionResult Download(Guid id)
        {
            return this.File([], "application/zip", $"{id}.zip");
        }

        [HttpPost("export")]
        [ProducesResponseType<FileContentResult>(StatusCodes.Status200OK)]
        public IActionResult Export([FromBody] Guid[] ids)
        {
            return this.File([], "application/zip", "export.zip");
        }
    }
}
