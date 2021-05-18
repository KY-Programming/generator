using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using ServiceFromSignalRViaFluentApi.Controllers;

namespace ServiceFromSignalRViaFluentApi.Hubs
{
    public interface IWeatherForecastHub
    {
        Task Refreshed(IEnumerable<WeatherForecast> forecast);
    }

    public class WeatherForecastHub : Hub<IWeatherForecastHub>
    {
        public void Refresh()
        {
            var rng = new Random();
            WeatherForecast[] forecast = Enumerable.Range(1, 5).Select(index => new WeatherForecast
                                                                                {
                                                                                    Date = DateTime.Now.AddDays(index),
                                                                                    TemperatureC = rng.Next(-20, 55),
                                                                                    Summary = WeatherForecastController.Summaries[rng.Next(WeatherForecastController.Summaries.Length)]
                                                                                })
                                                   .ToArray();

            this.Clients.All.Refreshed(forecast);
        }
    }
}