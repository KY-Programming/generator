using KY.Generator;
using Microsoft.AspNetCore.Mvc;

namespace ServiceFromAspNetCoreAnnotation.Controllers;

/// <summary>
/// [GenerateAngularService] is all that is needed - KY.Generator reads this controller during
/// the build and writes the Angular service and its models into the ClientApp.
/// The attribute says only *what* to generate. Output folders default to
/// ClientApp/src/app/{services,models} and can be redirected project-wide from AssemblyInfo.cs.
/// </summary>
[GenerateAngularService]
[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
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
                                              TemperatureC = Random.Shared.Next(-20, 55),
                                              Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                                          })
                         .ToArray();
    }
}
