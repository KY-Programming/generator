using System.Collections.Generic;

namespace AspDotNetAnnotations.Models
{
    public class GetComplexModel
    {
        public string Property { get; set; } = "";

        public GetComplexModelService Service { get; set; } = new();
        public List<GetComplexModelService> Services { get; set; } = [];
    }

    public class GetComplexModelService
    {
        public string Property { get; set; } = "";
    }
}