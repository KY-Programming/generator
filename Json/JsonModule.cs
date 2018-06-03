using KY.Core.Dependency;
using KY.Core.Module;
using KY.Generator.Configuration;
using KY.Generator.Json.Configuration;
using KY.Generator.Json.Extensions;
using KY.Generator.Mappings;

namespace KY.Generator.Json
{
    public class JsonModule : ModuleBase
    {
        public JsonModule(IDependencyResolver dependencyResolver)
            : base(dependencyResolver)
        { }

        public override void Initialize()
        {
            this.DependencyResolver.Bind<IConfigurationReader>().To<JsonConfigurationReader>();
            this.DependencyResolver.Bind<IGenerator>().To<JsonGenerator>();
            this.DependencyResolver.Get<ITypeMapping>().Initialize();
        }
    }
}