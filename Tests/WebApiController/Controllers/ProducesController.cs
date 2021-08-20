using System;
using System.Collections.Generic;
using System.Linq;
using KY.Generator;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiController.Models;

namespace WebApiController.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [GenerateAngularService("ClientApp/src/app/produces/services", "ClientApp/src/app/produces/models")]
    public class ProducesController : ControllerBase
    {
        private static readonly string[] summaries = { "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching" };

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<WeatherForecast>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult Get(int days)
        {
            if (days <= 0)
            {
                return this.NoContent();
            }
            return this.Ok(this.Random(5));
        }

        [HttpGet("[action]")]
        [Produces(typeof(IEnumerable<WeatherForecast>))]
        public IActionResult Get2(int days)
        {
            return this.Get(days);
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
