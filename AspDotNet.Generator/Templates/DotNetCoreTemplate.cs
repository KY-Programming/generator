using System.Collections.Generic;
using KY.Generator.Templates;

namespace KY.Generator.AspDotNet.Templates
{
    internal class DotNetCoreTemplate : ITemplate
    {
        public string Name => ".net Core 2.0";

        public IEnumerable<UsingTemplate> Usings
        {
            get
            {
                yield return new UsingTemplate("System.Collections.Generic", null, null);
                yield return new UsingTemplate("Microsoft.AspNetCore.Mvc", null, null);
            }
        }

        public bool UseOwnCache => true;
        public bool UseAttributes => true;
        public bool ValidateInput => true;
        public string ControllerBase => "ControllerBase";
    }
}