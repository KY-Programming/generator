using Microsoft.AspNetCore.SignalR;
using ServiceFromAspNetCoreSignalRHub.Hubs;
using ServiceFromAspNetCoreSignalRHub.Models;

namespace ServiceFromAspNetCoreSignalRHub.Services;

public class WeatherService
{
    private static readonly string[] Summaries =
    [
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    ];

    private readonly IHubContext<WeatherHub, IWeatherHub> hubContext;
    private WeatherForecast[] forecast = [];

    public WeatherService(IHubContext<WeatherHub, IWeatherHub> hubContext)
    {
        this.hubContext = hubContext;
        this.Fetch();
    }

    public IEnumerable<WeatherForecast> Get()
    {
        return this.forecast;
    }

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
