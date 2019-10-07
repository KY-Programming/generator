using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using KY.Core;
using KY.Core.Dependency;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KY.Generator.Configuration
{
    //internal class ConfigurationReaderVersion1 : IConfigurationReaderVersion
    //{
    //    private readonly IDependencyResolver resolver;
    //    private readonly List<string> ignoredNames;

    //    public int Version => 1;

    //    public ConfigurationReaderVersion1(IDependencyResolver resolver)
    //    {
    //        this.resolver = resolver;
    //        this.ignoredNames = new List<string> { "VerifySSL", "Language", "AddHeader" };
    //    }

    //    public List<ConfigurationPair> Read(JToken token)
    //    {
    //        string xml = $"{{ \"Configuration\": {token} }}";
    //        XElement element = JsonConvert.DeserializeXNode(xml).Root;
    //        return this.Read(element);
    //    }

    //    public List<ConfigurationPair> Read(XElement element)
    //    {
    //        List<ConfigurationPair> list = new List<ConfigurationPair>();
    //        foreach (XElement configurationElement in element.Elements())
    //        {
    //            string name = configurationElement.Name.LocalName;
    //            if (this.ignoredNames.Contains(name))
    //            {
    //                continue;
    //            }
    //            IConfigurationReader reader = this.resolver.Get<IEnumerable<IConfigurationReader>>().FirstOrDefault(x => x.TagName == name);
    //            if (reader == null)
    //            {
    //                Logger.Trace($"Skipped element with name {name}: No generator found");
    //                continue;
    //            }
    //            ConfigurationBase configuration = reader.Read(element, configurationElement);
    //            if (configuration != null)
    //            {
    //                ConfigurationPair pair = new ConfigurationPair();
    //                pair.Writers.Add(configuration);
    //                list.Add(pair);
    //            }
    //        }
    //        return list;
    //    }
    //}
}