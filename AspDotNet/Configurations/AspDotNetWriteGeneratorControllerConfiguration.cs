using System.Collections.Generic;

namespace KY.Generator.AspDotNet.Configurations
{
    internal class AspDotNetGeneratorControllerConfiguration
    {
        public string RelativePath { get; set; }
        public string Namespace { get; set; }
        public List<string> Usings { get; set; }
        public List<string> PreloadModules { get; set; }
        public List<AspDotNetControllerConfigureModule> Configures { get; set; }

        public AspDotNetGeneratorControllerConfiguration()
        {
            this.PreloadModules = new List<string>();
            this.Configures = new List<AspDotNetControllerConfigureModule>();
            this.Usings = new List<string>();
        }
    }
}