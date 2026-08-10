using AspDotNetAnnotations.Hubs;
using AspDotNetAnnotations.Models;
using Microsoft.AspNetCore.SignalR;

namespace AspDotNetAnnotations.Services;

/// <summary>Pushes forecasts to the hub clients; only there so the hub has something to send.</summary>
public class WeatherForecastService
{
    private static readonly string[] Summaries =
    [
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    ];

    private readonly IHubContext<WeatherForecastHub, IWeatherForecastHub> hubContext;
    private WeatherForecast[] forecast = [];

    public WeatherForecastService(IHubContext<WeatherForecastHub, IWeatherForecastHub> hubContext)
    {
        this.hubContext = hubContext;
        this.Fetch();
    }

    public IEnumerable<WeatherForecast> Get() => this.forecast;

    public void Fetch()
    {
        this.forecast = Enumerable.Range(1, 5)
                                  .Select(index => new WeatherForecast
                                                   {
                                                       Date = DateTime.Now.AddDays(index),
                                                       TemperatureC = Random.Shared.Next(-20, 55),
                                                       Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                                                   })
                                  .ToArray();
        this.hubContext.Clients.All.Updated(this.forecast);
    }
}
