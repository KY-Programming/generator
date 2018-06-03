using System.Collections.Generic;
using KY.Generator.Templates;

namespace KY.Generator.Json.Configuration
{
    public class JsonToObjectReaderConfiguration
    {
        public string Name { get; set; }
        public string RelativePath { get; set; }
        public string Namespace { get; set; }
        public List<ClassTemplate> WrittenObjects { get; }

        public JsonToObjectReaderConfiguration()
        {
            this.WrittenObjects = new List<ClassTemplate>();
        }
    }
}