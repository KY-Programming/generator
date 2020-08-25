using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServiceFromAspNetCoreSignalRHub.Models;
using ServiceFromAspNetCoreSignalRHub.Services;

namespace ServiceFromAspNetCoreSignalRHub.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
      private readonly ILogger<WeatherForecastController> _logger;
        private readonly WeatherService service;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, WeatherService service)
        {
          _logger = logger;
          this.service = service;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            return this.service.Get();
        }
    }
}
