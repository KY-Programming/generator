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
///
/// ASP.NET's own [Produces] is honoured too, so an action typed as IActionResult still gets a
/// properly typed Angular method - see <see cref="ProducesGet" />.
/// </summary>
[ApiController]
[Route("[controller]")]
[GenerateAngularService(name: "{0}ApiService")]
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
    /// IActionResult carries no type information, so on its own the generated method would return
    /// nothing useful. ASP.NET's [Produces(typeof(...))] declares the real response type and the
    /// generator reads it, so the Angular method still returns WeatherForecast[].
    ///
    /// This needs no KY.Generator attribute at all - the annotation you already write for
    /// Swagger/OpenAPI is enough. [ProducesResponseType] works the same way; both are only read
    /// for status code 200.
    /// </summary>
    [HttpGet("produces")]
    [Produces(typeof(WeatherForecast[]))]
    public IActionResult ProducesGet()
    {
        return this.Ok(this.Get());
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
