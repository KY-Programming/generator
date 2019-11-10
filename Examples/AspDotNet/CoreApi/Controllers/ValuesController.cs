using System.Collections.Generic;
using KY.Generator.Examples.AspDotNetCore.Models;
using Microsoft.AspNetCore.Mvc;

namespace KY.Generator.Examples.AspDotNetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
    }
}