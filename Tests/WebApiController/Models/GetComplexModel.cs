using System.Collections.Generic;

namespace WebApiController.Models
{
    public class GetComplexModel
    {
        public string Property { get; set; }

        public GetComplexModelService Service { get; set; }
        public List<GetComplexModelService> Services { get; set; }
    }

    public class GetComplexModelService
    {
        public string Property { get; set; }
    }
}