using KY.Core.Dependency;
using KY.Core.Module;
using KY.Generator.Configuration;
using KY.Generator.Xml.Configuration;

namespace KY.Generator.Xml
{
    public class XmlModule : ModuleBase
    {
        public XmlModule(IDependencyResolver dependencyResolver)
            : base(dependencyResolver)
        { }

        public override void Initialize()
        {
            this.DependencyResolver.Bind<IConfigurationReader>().To<XmlConfigurationReader>();
            this.DependencyResolver.Bind<IGenerator>().To<XmlGenerator>();
            //this.DependencyResolver.Get<ITypeMapping>().Initialize();
        }
    }
}