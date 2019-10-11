using System.Collections.Generic;
using KY.Generator.Languages;

namespace KY.Generator.Transfer
{
    // TODO: Only temporary location. Move to better location
    public class AspDotNetController : ITransferObject
    {
        public string Name { get; set; }
        public string Route { get; set; }
        public ILanguage Language { get; set; }

        public List<AspDotNetControllerAction> Actions { get; }

        public AspDotNetController()
        {
            this.Actions = new List<AspDotNetControllerAction>();
        }
    }

    public class AspDotNetControllerAction
    {
        public string Name { get; set; }
        public TypeTransferObject ReturnType { get; set; }
        public string Route { get; set; }
        public bool RequireBodyParameter { get; set; }
        public List<AspDotNetControllerActionParameter> Parameters { get; }
        public AspDotNetControllerActionType Type { get; set; }

        public AspDotNetControllerAction()
        {
            this.Parameters = new List<AspDotNetControllerActionParameter>();
        }
    }

    public class AspDotNetControllerActionParameter
    {
        public string Name { get; set; }
        public bool FromBody { get; set; }
        public TypeTransferObject Type { get; set; }
    }

    public enum AspDotNetControllerActionType {
        Get,
        Post,
        Put,
        Patch,
        Delete
    }
}