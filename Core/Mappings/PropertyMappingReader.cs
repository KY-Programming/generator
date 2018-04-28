using System.Collections.Generic;
using System.Xml.Linq;
using KY.Core.DataAccess;

namespace KY.Generator.Mappings
{
    internal class PropertyMappingReader
    {
        public IEnumerable<PropertyMapping> Read(XElement element)
        {
            IEnumerable<XElement> mapElements = element.Elements("Map");
            foreach (XElement mapElement in mapElements)
            {
                string name = mapElement.TryGetStringAttribute("Property");
                if (string.IsNullOrEmpty(name))
                {
                    continue;
                }
                string to = mapElement.TryGetStringAttribute("To");
                yield return new PropertyMapping(name, to);
            }
            IEnumerable<XElement> ignoreElements = element.Elements("Ignore");
            foreach (XElement mapElement in ignoreElements)
            {
                string name = mapElement.TryGetStringAttribute("Property");
                if (string.IsNullOrEmpty(name))
                {
                    continue;
                }
                yield return new PropertyMapping(name);
            }
        }
    }
}