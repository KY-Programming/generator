using System;
using System.Collections.Generic;
using System.Linq;
using KY.Generator;
using Microsoft.AspNetCore.Mvc;
using WebApiController.Models;

namespace WebApiController.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("[controller]")]
    [GenerateAngularService("ClientApp/src/app/versioned-api/services", "ClientApp/src/app/versioned-api/models")]
    public class VersionedApiController : ControllerBase
    {
        private static readonly string[] summaries = { "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching" };

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            return this.Random(5);
        }

        [HttpGet("next/{days}/days")]
        public IEnumerable<WeatherForecast> GetNext(int days)
        {
            return this.Random(days);
        }

        [HttpGet("next-days")]
        public IEnumerable<WeatherForecast> GetNext2(int days)
        {
            return this.Random(days);
        }

        private IEnumerable<WeatherForecast> Random(int days)
        {
            var rng = new Random();
            return Enumerable.Range(1, days).Select(index => new WeatherForecast
                                                             {
                                                                 Date = DateTime.Now.AddDays(index),
                                                                 TemperatureC = rng.Next(-20, 55),
                                                                 Summary = summaries[rng.Next(summaries.Length)]
                                                             })
                             .ToArray();
        }
    }
}