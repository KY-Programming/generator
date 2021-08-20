using KY.Generator;
using Microsoft.AspNetCore.Mvc;
using WebApiController.Models;

namespace WebApiController.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [GenerateAngularService("ClientApp/src/app/get-complex/services", "ClientApp/src/app/get-complex/models")]
    public class GetComplexController
    {
        [HttpGet("[action]")]
        public GetComplexModel Get()
        {
            return new GetComplexModel
                   {
                       Property = "Tut1",
                       Service = new GetComplexModelService
                                 {
                                     Property = "Tut2"
                                 },
                       Services =
                       {
                           new GetComplexModelService
                           {
                               Property = "Tut3"
                           },
                           new GetComplexModelService
                           {
                               Property = "Tut5"
                           }
                       }
                   };
        }
    }
}
