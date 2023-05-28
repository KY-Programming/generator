using System.Collections.Generic;

namespace WebApiController.Models
{
    public class SelfReferencingModel
    {
        public string Name { get; set; }
        public List<SelfReferencingModel> Children { get; } = new List<SelfReferencingModel>();
    }
}
