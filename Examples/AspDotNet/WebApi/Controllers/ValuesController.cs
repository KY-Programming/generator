using System.Collections.Generic;
using System.Web.Http;
using KY.Generator.Examples.AspDotNet.Models;

namespace KY.Generator.Examples.AspDotNet.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet]
        public Value Get(int id)
        {
            return new Value { Id = id, Text = "value" };
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] Value value)
        { }

        // PUT api/values/5
        [HttpPut]
        public void Put(int id, [FromBody] Value value)
        { }

        // DELETE api/values/5
        [HttpDelete]
        public void Delete(int id)
        { }
    }
}