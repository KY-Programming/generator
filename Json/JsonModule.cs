using System.Runtime.CompilerServices;
using KY.Core.Dependency;
using KY.Core.Module;
using KY.Generator.Command;
using KY.Generator.Json.Commands;
using KY.Generator.Json.Extensions;
using KY.Generator.Mappings;

[assembly: InternalsVisibleTo("KY.Generator.Json.Tests")]

namespace KY.Generator.Json;

public class JsonModule : ModuleBase
{
    public JsonModule(IDependencyResolver dependencyResolver)
        : base(dependencyResolver)
    {
        this.DependencyResolver.Get<GeneratorCommandFactory>().Register<JsonReadCommand>(JsonReadCommand.Names);
        this.DependencyResolver.Get<GeneratorCommandFactory>().Register<JsonWriteCommand>(JsonWriteCommand.Names);
    }

    public override void Initialize()
    {
        this.DependencyResolver.Get<ITypeMapping>().Initialize();
    }
}
