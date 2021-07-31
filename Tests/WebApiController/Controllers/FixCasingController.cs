using System.Buffers;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using KY.Core;
using KY.Generator;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Formatters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using WebApiController.Models;

namespace WebApiController.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [GenerateAngularService("ClientApp/src/app/fix-casing/services", "ClientApp/src/app/fix-casing/models")]
    [GenerateOption(GenerateOption.SkipHeader)]
    [FixCasingController]
    public class FixCasingController : ControllerBase
    {
        [HttpGet("[action]")]
        public CasingModel Get()
        {
            return new CasingModel();
        }

        [HttpPost("[action]")]
        public void Post(CasingModel model)
        { }

        [HttpGet("[action]")]
        public CasingWithMappingModel GetWithMapping()
        {
            return new CasingWithMappingModel();
        }

        [HttpGet("[action]")]
        public IEnumerable<CasingWithMappingModel> GetArrayWithMapping()
        {
            return new CasingWithMappingModel().Yield();
        }

        [HttpPost("[action]")]
        public void PostWithMapping(CasingWithMappingModel model)
        { }
    }

    public class FixCasingControllerAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext ctx)
        {
            if (ctx.ActionDescriptor is ControllerActionDescriptor cad && cad.ActionName == nameof(FixCasingController.GetWithMapping) && ctx.Result is ObjectResult objectResult)
            {
                ctx.Result = new CustomObjectResult(ctx.Result.CastTo<ObjectResult>().Value);
            }
        }
    }

    public class CustomObjectResult : ObjectResult
    {
        public override async Task ExecuteResultAsync(ActionContext context)
        {
             // await base.ExecuteResultAsync(context);
             await context.HttpContext.Response.WriteAsync(
                 "{" +
                 "  \"alllower\": \"mrUePWtWEhjf0VUMY7KVJ\"," +
                 "  \"ALLUPPER\": \"4QhZdr3Y?ws9fUdB58VNU97\"," +
                 "  \"PascalCase\": \"?DqsQmuqqrAd93Sb\"," +
                 "  \"camelCase\": \"ThZZ4Q?JxbOa9n7WIZNbLh\"," +
                 "  \"snake_case\": \"bbT0u?EcpUUb1hZi0qWt\"," +
                 "  \"UPPER_SNAKE_CASE\": \"SqueFuBd\"" +
                 "}");
        }

        public CustomObjectResult(object value)
            : base(value)
        { }
    }
}
