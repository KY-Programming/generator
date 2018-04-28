using System.Xml.Linq;

namespace KY.Generator.Configuration
{
    public interface IConfigurationReader
    {
        string TagName { get; }
        ConfigurationBase Read(XElement rootElement, XElement configurationElement);
    }
}