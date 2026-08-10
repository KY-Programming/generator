using AspDotNetAnnotations.Models;
using AspDotNetAnnotations.Services;
using KY.Generator;
using Microsoft.AspNetCore.SignalR;

namespace AspDotNetAnnotations.Hubs;

public interface IWeatherForecastHub
{
    Task Updated(IList<WeatherForecast> forecast);
}

/// <summary>
/// A hub with a typed client interface and a callable method. GenerateWithRetry adds the reconnect
/// timeouts to the generated service, which is the part of the Angular hub service only this hub covers.
/// </summary>
[GenerateAngularHub("ClientApp/src/app/weather-hub/services", "ClientApp/src/app/weather-hub/models")]
[GenerateWithRetry(true, 0, 0, 1000, 2000, 5000)]
public class WeatherForecastHub : Hub<IWeatherForecastHub>
{
    private readonly WeatherForecastService service;

    public WeatherForecastHub(WeatherForecastService service)
    {
        this.service = service;
    }

    public override async Task OnConnectedAsync()
    {
        await this.Clients.Caller.Updated(this.service.Get().ToList());
        await base.OnConnectedAsync();
    }

    public void Fetch() => this.service.Fetch();
}
