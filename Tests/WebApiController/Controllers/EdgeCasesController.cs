﻿using System;
using System.Collections.Generic;
using System.Threading;
using KY.Generator;
using Microsoft.AspNetCore.Mvc;
using WebApiController.Models;
using WebApiController.Services;

namespace WebApiController.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    [GenerateAngularService("ClientApp/src/app/edge-cases/services", "ClientApp/src/app/edge-cases/models")]
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

        [HttpGet("[action]")]
        public string FromQuery([FromQuery] string queryValue = null, int value = 0)
        {
            return $"Result: {queryValue} + {value}";
        }

        [HttpGet("[action]")]
        public string FromQueryArray([FromQuery] List<string> queryArray)
        {
            return $"Result: {string.Join(", ", queryArray)}";
        }

        [HttpGet("[action]")]
        public GenericResult<string> GenericResult(string value1, string value2)
        {
            return new GenericResult<string>(new List<string> { value1, value2 });
        }

        [HttpGet("[action]")]
        public GenericResult<ExclusiveGenericComplexResult> GenericComplexResult()
        {
            return new GenericResult<ExclusiveGenericComplexResult>(new List<ExclusiveGenericComplexResult> { new ExclusiveGenericComplexResult() });
        }

        [HttpGet("[action]")]
        public GenericResult<DateModel> GetGenericWithModel()
        {
            return new GenericResult<DateModel>(new List<DateModel> { new DateModel() });
        }

        [HttpGet("[action]")]
        public string GetWithOptional(int required, string optional = null)
        {
            return required + " " + (optional ?? "null");
        }

        [HttpGet("[action]/{required}/{optional?}")]
        public string GetInlineWithOptional(int required, string optional = null)
        {
            return required + " " + (optional ?? "null");
        }

        [HttpGet("[action]/required/{required}/optional/{optional?}")]
        public string GetNamedInlineWithOptional(int required, string optional = null)
        {
            return required + " " + (optional ?? "null");
        }

        [HttpGet("/api/test/[controller]/[action]")]
        public string GetWithAbsoluteRoute()
        {
            return "works";
        }

        [HttpGet("/api/test/{id}/[action]/[controller]")]
        public string GetWithAbsoluteRouteAndParameter(int id)
        {
            return "works " + id;
        }

        [HttpGet("[action]")]
        public object UnknownResult(string value)
        {
            return new UnknownResult(value);
        }

        [HttpGet("[action]")]
        public SelfReferencingModel SelfReferencing()
        {
            return new SelfReferencingModel();
        }
    }

    public class ExclusiveGenericComplexResult
    { }

    class UnknownResult
    {
        public string Value { get; }

        public UnknownResult(string value)
        {
            this.Value = value;
        }
    }
}
