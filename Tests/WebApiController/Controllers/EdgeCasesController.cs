﻿using System;
using System.Collections.Generic;
using System.Threading;
using KY.Generator;
using Microsoft.AspNetCore.Mvc;
using WebApiController.Services;

namespace WebApiController.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [GenerateAngularService("ClientApp/src/app/edge-cases/services", "ClientApp/src/app/edge-cases/models")]
    [GenerateOption(GenerateOption.SkipHeader)]
    public class EdgeCasesController : ControllerBase
    {
        [HttpGet("[action]")]
        public void Get(string subject)
        { }

        [HttpPost("[action]")]
        public void Post(string subject)
        { }

        [HttpGet("[action]")]
        public List<string> Cancelable(string subject, CancellationToken token)
        {
            return new List<string> { subject };
        }

        [HttpGet("[action]")]
        public string String()
        {
            return "Hello World!";
        }

        [HttpGet("[action]")]
        public Guid GetGuid()
        {
            return Guid.NewGuid();
        }

        [HttpGet("[action]")]
        public bool WithDI([FromServices] DummyService service, int value)
        {
            service.Action();
            return value == 0;
        }

        [HttpGet("[action]")]
        public string FromHeader([FromHeader] string headerValue = null, int value = 0)
        {
            return $"Result: {headerValue} + {value}";
        }
    }
}
