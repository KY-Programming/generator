using Microsoft.AspNetCore.SignalR;
using SignalR.Hubs;

namespace SignalR.Services
{
    public class WeatherForecastService
    {
        private readonly IHubContext<WeatherForecastHub, IWeatherForecastHub> hubContext;
        private static readonly string[] summaries = { "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching" };
        private WeatherForecast[] forecast = Array.Empty<WeatherForecast>();

        public WeatherForecastService(IHubContext<WeatherForecastHub, IWeatherForecastHub> hubContext)
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
