using KY.Generator;
using Microsoft.AspNetCore.SignalR;
using ServiceFromAspNetCoreSignalRHub.Models;
using ServiceFromAspNetCoreSignalRHub.Services;

namespace ServiceFromAspNetCoreSignalRHub.Hubs;

/// <summary>
/// The client-callback contract. Every method here becomes an observable on the generated
/// Angular hub service.
/// </summary>
public interface IWeatherHub
{
    Task Updated(IList<WeatherForecast> forecast);
}

/// <summary>
/// [GenerateAngularHub] generates an Angular service that opens the SignalR connection,
/// exposes Updated as an observable and forwards Fetch to the server.
/// [GenerateWithRetry] bakes the reconnect delays (in milliseconds) into that service.
/// Output folders default to ClientApp/src/app/{services,models} and can be redirected
/// project-wide from AssemblyInfo.cs.
/// </summary>
[GenerateAngularHub]
[GenerateWithRetry(true, 0, 0, 1000, 2000, 5000)]
public class WeatherHub : Hub<IWeatherHub>
{
    private readonly WeatherService service;

    public WeatherHub(WeatherService service)
    {
        this.service = service;
    }

    public override async Task OnConnectedAsync()
    {
        await this.Clients.Caller.Updated(this.service.Get().ToList());
        await base.OnConnectedAsync();
    }

    public void Fetch()
    {
        this.service.Fetch();
    }
}
