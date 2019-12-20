using System.Runtime.CompilerServices;
using KY.Core.Dependency;
using KY.Core.Module;
using KY.Generator.Configuration;
using KY.Generator.Json.Configurations;
using KY.Generator.Json.Extensions;
using KY.Generator.Json.Readers;
using KY.Generator.Json.Writers;
using KY.Generator.Mappings;

[assembly: InternalsVisibleTo("KY.Generator.Json.Tests")]

namespace KY.Generator.Json
{
    public class JsonModule : ModuleBase
    {
        public JsonModule(IDependencyResolver dependencyResolver)
            : base(dependencyResolver)
        { }

        public override void Initialize()
        {
            this.DependencyResolver.Get<ITypeMapping>().Initialize();
            this.DependencyResolver.Get<ConfigurationMapping>()
                .Map<JsonReadConfiguration, JsonReader>("json")
                .Map<JsonWriteConfiguration, JsonWriter>("json");
        }
    }
}