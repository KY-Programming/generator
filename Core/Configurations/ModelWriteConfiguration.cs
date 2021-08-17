using System.Collections.Generic;

namespace KY.Generator.Configurations
{
    public class ModelWriteConfiguration : IModelConfiguration
    {
        public string Name { get; set; }
        public string Namespace { get; set; }
        public string RelativePath { get; set; }
        public List<string> Usings { get; set; }

        public ModelWriteConfiguration()
        {
            this.Usings = new List<string>();
        }
    }
}
