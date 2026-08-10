using Asp.Versioning;
using AspDotNetAnnotations.Models;
using KY.Generator;
using Microsoft.AspNetCore.Mvc;

namespace AspDotNetAnnotations.Controllers;

/// <summary>
/// The version is part of the route template as {version:apiVersion}. The generated service has to resolve
/// that token from the ApiVersion attribute instead of turning it into a method parameter. The controller
/// supports two versions, so the action mapped to 2.0 has to come out under that version rather than the
/// controller default, and the action with an absolute route has to keep its own template while still
/// resolving the token.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[GenerateAngularService("ClientApp/src/app/versioned-api/services", "ClientApp/src/app/versioned-api/models")]
public class VersionedApiController : ControllerBase
{
    private static readonly string[] Summaries =
    [
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    ];

    [HttpGet]
    public IEnumerable<WeatherForecast> Get()
    {
        return Random(5);
    }

    /// <summary>A multi segment route template with a parameter in the middle.</summary>
    [HttpGet("next/{days}/days")]
    public IEnumerable<WeatherForecast> GetNext(int days)
    {
        return Random(days);
    }

    /// <summary>Only reachable on 2.0, so the generated call has to use that version.</summary>
    [HttpGet("next-days")]
    [MapToApiVersion("2.0")]
    public IEnumerable<WeatherForecast> GetNext2(int days)
    {
        return Random(days);
    }

    /// <summary>An absolute route that escapes the controller route but keeps the version token.</summary>
    [HttpGet("/api/v{version:apiVersion}/test/[controller]/[action]")]
    public string GetWithAbsoluteRoute()
    {
        return "works";
    }

    private static IEnumerable<WeatherForecast> Random(int days)
    {
        return Enumerable.Range(1, days)
                         .Select(index => new WeatherForecast
                                          {
                                              Date = DateTime.Now.AddDays(index),
                                              TemperatureC = System.Random.Shared.Next(-20, 55),
                                              Summary = Summaries[System.Random.Shared.Next(Summaries.Length)]
                                          })
                         .ToArray();
    }
}
