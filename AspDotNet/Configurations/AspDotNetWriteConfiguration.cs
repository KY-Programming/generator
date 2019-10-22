using System.Collections.Generic;
using KY.Generator.AspDotNet.Templates;
using KY.Generator.Configuration;
using KY.Generator.Csharp.Languages;

namespace KY.Generator.AspDotNet.Configurations
{
    public class AspDotNetWriteConfiguration : ConfigurationBase
    {
        public string Namespace { get; set; }
        public string RelativePath { get; set; }
        public bool FormatNames { get; set; }
        public AspDotNetGeneratorControllerConfiguration GeneratorController { get; set; }
        public List<AspDotNetWriteEntityControllerConfiguration> Controllers { get; set; }
        public List<string> Usings { get; set; }
        internal ITemplate Template { get; set; }

        public AspDotNetWriteConfiguration()
        {
            this.Language = CsharpLanguage.Instance;
            this.FormatNames = true;
            this.Controllers = new List<AspDotNetWriteEntityControllerConfiguration>();
            this.Usings = new List<string>();
            this.Template = new DotNetFrameworkTemplate();
        }
    }
}