using System.Collections.Generic;
using KY.Generator.Templates;

namespace KY.Generator.AspDotNet.Templates
{
    internal class DotNetFrameworkTemplate : ITemplate
    {
        public string Name => ".net 4.5";

        public IEnumerable<UsingTemplate> Usings
        {
            get
            {
                yield return new UsingTemplate("System.Collections.Generic", null, null);
                yield return new UsingTemplate("System.Web.Http", null, null);
            }
        }

        public bool UseOwnCache => false;
        public bool UseAttributes => false;
        public bool ValidateInput => false;
        public string ControllerBase => "ApiController";
    }
}