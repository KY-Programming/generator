using ChangeReturnType.Models;
using KY.Generator;
using Microsoft.AspNetCore.Mvc;

namespace ChangeReturnType.Controllers;

/// <summary>
/// Shows how to override what the generated Angular service returns.
///
/// [GenerateMethod(Type = ...)] swaps a method's generated return type, and [GenerateImport]
/// tells the generator where that type comes from on the TypeScript side instead of generating
/// it. Two flavours are demonstrated:
///   - CustomWeatherForecast is a hand written file inside the app (ClientApp/src/app/models).
///   - SpecialWeatherForecast comes from a shared library imported as "@my-lib/models".
/// </summary>
[ApiController]
[Route("[controller]")]
[GenerateAngularService("/ClientApp/src/app/services", "/ClientApp/src/app/models", "{0}ApiService")]
[GenerateImport(typeof(SpecialWeatherForecast), "@my-lib/models", "SpecialWeatherForecast")]
[GenerateImport(typeof(WeatherForecast), "../models", "CustomWeatherForecast as WeatherForecast")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries =
    [
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    ];

    /// <summary>
    /// The action returns WeatherForecast, but the generated service declares
    /// CustomWeatherForecast[] - the richer type the Angular app actually wants to work with.
    /// </summary>
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

    /// <summary>
    /// SpecialWeatherForecast is never generated - the service imports it from "@my-lib/models".
    /// </summary>
    [HttpGet("special")]
    public IEnumerable<SpecialWeatherForecast> SpecialGet()
    {
        return
        [
            new SpecialWeatherForecast
            {
                Date = DateTime.Now,
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)],
                Warning = "Severe weather expected"
            }
        ];
    }
}
