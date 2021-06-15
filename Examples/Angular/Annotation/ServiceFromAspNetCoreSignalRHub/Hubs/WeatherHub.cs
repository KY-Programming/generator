using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KY.Generator;
using Microsoft.AspNetCore.SignalR;
using ServiceFromAspNetCoreSignalRHub.Models;
using ServiceFromAspNetCoreSignalRHub.Services;

namespace ServiceFromAspNetCoreSignalRHub.Hubs
{
    public interface IWeatherHub
    {
        Task Updated(IList<WeatherForecast> forecast);
    }

    [GenerateAngularHub("\\ClientApp\\src\\app\\services", "\\ClientApp\\src\\app\\models")]
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
            await this.Clients.Caller.Updated(this.service.Get()?.ToList());
            await base.OnConnectedAsync();
        }

        public void Fetch()
        {
            this.service.Fetch();
        }
    }
}
