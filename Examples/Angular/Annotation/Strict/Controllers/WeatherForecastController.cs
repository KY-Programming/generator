using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KY.Generator;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Strict.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [GenerateAngularService(@"ClientApp\src\app\services", @"ClientApp\src\app\models", "{0}ApiService")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
                                                     {
                                                         "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
                                                     };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
                                                          {
                                                              Date = DateTime.Now.AddDays(index),
                                                              TemperatureC = rng.Next(-20, 55),
                                                              Summary = Summaries[rng.Next(Summaries.Length)]
                                                          })
                             .ToArray();
        }
    }
}
