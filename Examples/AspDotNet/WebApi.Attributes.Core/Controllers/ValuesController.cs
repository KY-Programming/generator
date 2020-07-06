using System.Collections.Generic;
using KY.Generator.Examples.AspDotNet.Models;
using Microsoft.AspNetCore.Mvc;

namespace KY.Generator.Examples.AspDotNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [GenerateAngularService("..\\..\\Angular\\src\\app\\services", "..\\..\\Angular\\src\\app\\models", "ValuesCoreService")]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<Value> Get(int id)
        {
            return new Value { Id = id, Text = "value" };
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] Value value)
        { }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Value value)
        { }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        { }

        // GET api/values/check
        [HttpGet("[action]")]
        public ActionResult Check()
        {
            return this.Ok();
        }

        // GET api/values/checkinterface
        [HttpGet("[action]")]
        public IActionResult CheckInterface()
        {
            return this.Ok();
        }
    }
}