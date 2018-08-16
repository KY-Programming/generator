using System.Collections.Generic;
using KY.Generator.Configuration;
using KY.Generator.Templates;

namespace KY.Generator.Xml
{
    public class XmlGenerator : IGenerator
    {
        public List<FileTemplate> Files { get; }

        public XmlGenerator()
        {
            this.Files = new List<FileTemplate>();
        }

        public void Generate(ConfigurationBase configurationBase)
        {
            this.Files.Clear();
            XmlConfiguration configuration = configurationBase as XmlConfiguration;
            if (configuration == null)
            {
                return;
            }
        }
    }
}