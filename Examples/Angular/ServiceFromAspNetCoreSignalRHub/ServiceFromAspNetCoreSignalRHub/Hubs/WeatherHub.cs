using System.Collections.Generic;
using Microsoft.AspNetCore.SignalR;
using KY.Generator;

namespace ServiceFromAspNetCoreSignalRHub.Hubs
{
  public interface IWeatherHub
  {
    void Updated(IList<WeatherForecast> forecast);
  }
  
  [GenerateAngularHub("\\GardenClientApp\\src\\app\\services", "\\GardenClientApp\\src\\app\\models")]
  [GenerateWithRetry(true, 0, 0, 1000, 2000, 5000)]
  public class WeatherHub : Hub<IWeatherHub>
  {
    public void Fetch()
    { }
  }
}
