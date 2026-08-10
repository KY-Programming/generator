using KY.Generator;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace AspDotNetAnnotations.Controllers;

/// <summary>
/// A version neutral controller is served under every requested version, so no version may be pinned to its calls.
/// The route still carries the {version:apiVersion} token, which has to be filled with the default version to keep
/// the generated address reachable, and the generator has to warn that it picked one.
/// </summary>
[ApiController]
[ApiVersionNeutral]
[Route("api/v{version:apiVersion}/[controller]")]
[GenerateAngularService("ClientApp/src/app/api-version-neutral/services", "ClientApp/src/app/api-version-neutral/models")]
public class ApiVersionNeutralController : ControllerBase
{
    [HttpGet]
    public string Get()
    {
        return "works";
    }

    /// <summary>A route without the version token stays untouched.</summary>
    [HttpGet("/api/neutral/[controller]/[action]")]
    public string GetWithoutVersion()
    {
        return "works";
    }
}
