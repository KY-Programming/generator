using KY.Generator;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace AspDotNetAnnotations.Controllers;

/// <summary>
/// Api versions are not always major.minor: a version can carry the major part alone and it can carry a status
/// suffix. Both have to be written into the route exactly like the route constraint formats them, and the version
/// without status has to count as the newer one, so the unmapped action goes to 1 and only the mapped one to
/// 2.0-beta.
/// </summary>
[ApiController]
[ApiVersion("1")]
[ApiVersion("2.0-beta")]
[Route("api/v{version:apiVersion}/[controller]")]
[GenerateAngularService("ClientApp/src/app/status-api-version/services", "ClientApp/src/app/status-api-version/models")]
public class StatusApiVersionController : ControllerBase
{
    [HttpGet]
    public string Get()
    {
        return "works";
    }

    /// <summary>Only reachable on the prerelease version.</summary>
    [HttpGet("preview")]
    [MapToApiVersion("2.0-beta")]
    public string GetPreview()
    {
        return "works";
    }
}
