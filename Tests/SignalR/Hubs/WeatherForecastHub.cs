using KY.Generator;
using Microsoft.AspNetCore.SignalR;
using SignalR.Services;

namespace SignalR.Hubs;

public interface IWeatherForecastHub
{
    Task Updated(IList<WeatherForecast> forecast);
}

[GenerateAngularHub("\\ClientApp\\src\\app\\services", "\\ClientApp\\src\\app\\models")]
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

    public void Fetch()
    {
        this.service.Fetch();
    }
    
}
