using System.Runtime.CompilerServices;
using KY.Core.Dependency;
using KY.Core.Module;
using KY.Generator.Command;
using KY.Generator.Json.Commands;
using KY.Generator.Json.Extensions;
using KY.Generator.Mappings;

[assembly: InternalsVisibleTo("KY.Generator.Json.Tests")]

namespace KY.Generator.Json
{
    public class JsonModule : ModuleBase
    {
        public JsonModule(IDependencyResolver dependencyResolver)
            : base(dependencyResolver)
        {
            this.DependencyResolver.Bind<IGeneratorCommand>().To<JsonReadCommand>();
            this.DependencyResolver.Bind<IGeneratorCommand>().To<JsonWriteCommand>();
        }

        public override void Initialize()
        {
            this.DependencyResolver.Get<ITypeMapping>().Initialize();
        }
    }
}
