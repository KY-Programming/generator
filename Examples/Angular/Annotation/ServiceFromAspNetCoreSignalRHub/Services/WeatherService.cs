using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.SignalR;
using ServiceFromAspNetCoreSignalRHub.Hubs;
using ServiceFromAspNetCoreSignalRHub.Models;

namespace ServiceFromAspNetCoreSignalRHub.Services
{
    public class WeatherService
    {
        private readonly IHubContext<WeatherHub, IWeatherHub> hubContext;
        private static readonly string[] summaries = { "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching" };
        private WeatherForecast[] forecast;

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
            Random random = new Random();
            this.forecast = Enumerable.Range(1, 5).Select(index => new WeatherForecast
                                                                   {
                                                                       Date = DateTime.Now.AddDays(index),
                                                                       TemperatureC = random.Next(-20, 55),
                                                                       Summary = summaries[random.Next(summaries.Length)]
                                                                   })
                                      .ToArray();
            this.hubContext.Clients.All.Updated(this.forecast);
        }
    }
}
