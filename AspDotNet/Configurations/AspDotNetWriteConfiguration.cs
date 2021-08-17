using System.Collections.Generic;
using KY.Generator.AspDotNet.Templates;

namespace KY.Generator.AspDotNet.Configurations
{
    public class AspDotNetWriteConfiguration
    {
        public string Namespace { get; set; }
        public string RelativePath { get; set; }
        public AspDotNetGeneratorControllerConfiguration GeneratorController { get; set; }
        public List<AspDotNetWriteEntityControllerConfiguration> Controllers { get; set; }
        public List<string> Usings { get; set; }
        internal ITemplate Template { get; set; }

        public AspDotNetWriteConfiguration()
        {
            // this.Language = CsharpLanguage.Instance;
            this.Controllers = new List<AspDotNetWriteEntityControllerConfiguration>();
            this.Usings = new List<string>();
            this.Template = new DotNetFrameworkTemplate();
        }
    }
}
