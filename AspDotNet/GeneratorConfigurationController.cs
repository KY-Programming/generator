using System.Collections.Generic;

namespace KY.Generator.AspDotNet
{
    internal class GeneratorConfigurationController
    {
        public string RelativePath { get; set; }
        public string Namespace { get; set; }
        public List<string> PreloadModules { get; set; }

        public GeneratorConfigurationController()
        {
            this.PreloadModules = new List<string>();
        }
    }
}