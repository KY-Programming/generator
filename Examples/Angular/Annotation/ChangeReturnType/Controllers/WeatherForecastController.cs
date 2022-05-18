using KY.Generator;
using Microsoft.AspNetCore.Mvc;

namespace ChangeReturnType.Controllers;

[ApiController]
[Route("[controller]")]
[GenerateAngularService("/ClientApp/src/app/services", "/ClientApp/src/app/models", "{0}ApiService")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
                                                 {
                                                     "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
                                                 };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    [GenerateReturnType("CustomWeatherForecast[]", "custom-weather-forecast", "CustomWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
                                                      {
                                                          Date = DateTime.Now.AddDays(index),
                                                          TemperatureC = Random.Shared.Next(-20, 55),
                                                          Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                                                      })
                         .ToArray();
    }
}
