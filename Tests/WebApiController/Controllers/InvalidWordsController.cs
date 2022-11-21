using KY.Generator;
using Microsoft.AspNetCore.Mvc;
using WebApiController.Models;

namespace WebApiController.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    [GenerateAngularService("ClientApp/src/app/invalid-words/services", "ClientApp/src/app/invalid-words/models")]
    public class InvalidWordsController : ControllerBase
    {
        [HttpGet]
        public void Switch(string @switch)
        { }
        
        [HttpGet, HttpPost, HttpPatch, HttpPut, HttpDelete]
        public void Case(string @case)
        { }
    }
    
}
