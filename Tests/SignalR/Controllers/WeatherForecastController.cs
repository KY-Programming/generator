using Microsoft.AspNetCore.Mvc;
using SignalR.Services;

namespace SignalR.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;
    private readonly WeatherForecastService forecastService;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, WeatherForecastService forecastService)
    {
        _logger = logger;
        this.forecastService = forecastService;
    }

    [HttpGet]
    public IEnumerable<WeatherForecast> Get()
    {
        return this.forecastService.Get();
    }
}
