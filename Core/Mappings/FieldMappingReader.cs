using System.Collections.Generic;
using System.Xml.Linq;
using KY.Core.Xml;

namespace KY.Generator.Mappings
{
    internal class FieldMappingReader
    {
        public IEnumerable<FieldMapping> Read(XElement element)
        {
            IEnumerable<XElement> mapElements = element.Elements("Map");
            foreach (XElement mapElement in mapElements)
            {
                string name = mapElement.TryGetStringAttribute("Field");
                if (string.IsNullOrEmpty(name))
                {
                    continue;
                }
                string to = mapElement.TryGetStringAttribute("To");
                yield return new FieldMapping(name, to);
            }
            IEnumerable<XElement> ignoreElements = element.Elements("Ignore");
            foreach (XElement mapElement in ignoreElements)
            {
                string name = mapElement.TryGetStringAttribute("Field");
                if (string.IsNullOrEmpty(name))
                {
                    continue;
                }
                yield return new FieldMapping(name);
            }
        }
    }
}