using System.Collections.Generic;

namespace KY.Generator.AspDotNet
{
    internal class GeneratorConfigurationController
    {
        public string RelativePath { get; set; }
        public string Namespace { get; set; }
        public List<string> Usings { get; }
        public List<string> PreloadModules { get; }
        public List<GeneratorConfigurationConfigureModule> Configures { get; }

        public GeneratorConfigurationController()
        {
            this.PreloadModules = new List<string>();
            this.Configures = new List<GeneratorConfigurationConfigureModule>();
            this.Usings = new List<string>();
        }
    }
}