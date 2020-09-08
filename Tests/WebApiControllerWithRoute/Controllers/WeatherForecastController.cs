using System;
using System.Collections.Generic;
using System.Linq;
using KY.Generator;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApiControllerWithRoute.Models;

namespace WebApiControllerWithRoute.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [GenerateAngularService("\\ClientApp\\src\\app\\services", "\\ClientApp\\src\\app\\models")]
    [GenerateOption(GenerateOption.SkipHeader)]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
                                                     {
                                                         "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
                                                     };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            this._logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            return this.GetNext(5);
        }

        [HttpGet, Route("three")]
        public IEnumerable<WeatherForecast> GetThree()
        {
            return this.GetNext(3);
        }

        [HttpGet, Route("three/days")]
        public IEnumerable<WeatherForecast> GetThreeDays()
        {
            return this.GetNext(3);
        }


        [HttpGet, Route("next/{days}/days")]
        public IEnumerable<WeatherForecast> GetNext(int days)
        {
            var rng = new Random();
            return Enumerable.Range(1, days).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
                             .ToArray();
        }

        [HttpGet, Route("next/{days}/days/{test}")]
        public void Test(int test, int days)
        {

        }

        [HttpGet, Route("test2")]
        public void Test2(int test, int days)
        {

        }

        [HttpGet, Route("test3/{test}")]
        public void Test3(int test, int days)
        {

        }

        [HttpGet, Route("test4/{test}")]
        public void Test4(int days, int test)
        {

        }

        [HttpGet, HttpPost, Route("test5/{test}")]
        public void Test5(int test)
        {

        }

        [HttpGet, Route("test6/{test}")]
        public string[] Test6(int test)
        {
            return Enumerable.Empty<string>().ToArray();
        }

        [HttpGet, Route("test7/{test}")]
        [GenerateIgnoreGeneric(typeof(IgnoreMe<>))]
        public IgnoreMe<string[]> Test7(int test)
        {
            return new IgnoreMe<string[]>(Enumerable.Empty<string>().ToArray());
        }
    }
}