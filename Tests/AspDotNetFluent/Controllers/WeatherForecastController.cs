using Microsoft.AspNetCore.Mvc;

namespace AspDotNetFluent.Controllers;

/// <summary>
/// Carries no annotation on purpose - what reads it is the fluent entry point in Generator.cs.
/// </summary>
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
