using KY.Generator;
using Microsoft.AspNetCore.Mvc;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace EdgeCases;

/// <summary>
/// Action parameters named <c>url</c> or <c>httpOptions</c> must not collide with the locally-generated
/// URL string variable or the generated <c>httpOptions</c> parameter inside the produced TypeScript
/// service method body.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[GenerateAngularService("Output", "Output")]
public class HttpServiceParameterNamesController : ControllerBase
{
    [HttpPost("download")]
    public string Download(string url) => url;

    [HttpPost("with-http-options")]
    public string WithHttpOptions(string httpOptions) => httpOptions;
}
