using AspDotNetAnnotations.Models;
using KY.Generator;
using Microsoft.AspNetCore.Mvc;

namespace AspDotNetAnnotations.Controllers;

/// <summary>
/// The route carries the version token but the controller declares no ApiVersion, so the default applies.
/// Asp.Versioning serves such a controller under ApiVersion.Default, which is 1.0, and the generated
/// service has to call the same address rather than leave the token empty.
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[GenerateAngularService("ClientApp/src/app/default-api-version/services", "ClientApp/src/app/default-api-version/models")]
public class DefaultApiVersionController : ControllerBase
{
    private static readonly string[] Summaries =
    [
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    ];

    [HttpGet]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5)
                         .Select(index => new WeatherForecast
                                          {
                                              Date = DateTime.Now.AddDays(index),
                                              TemperatureC = System.Random.Shared.Next(-20, 55),
                                              Summary = Summaries[System.Random.Shared.Next(Summaries.Length)]
                                          })
                         .ToArray();
    }
}
