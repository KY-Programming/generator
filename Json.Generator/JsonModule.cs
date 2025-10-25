using System.Runtime.CompilerServices;
using KY.Core.Dependency;
using KY.Generator.Command;
using KY.Generator.Commands;
using KY.Generator.Csharp;
using KY.Generator.Json.Commands;
using KY.Generator.Json.Extensions;
using KY.Generator.Mappings;
using KY.Generator.Models;
using KY.Generator.TypeScript;

[assembly: InternalsVisibleTo("KY.Generator.Json.Tests")]

namespace KY.Generator.Json;

public class JsonModule : GeneratorModule
{
    public JsonModule(IDependencyResolver dependencyResolver)
        : base(dependencyResolver)
    {
        this.DependsOn<CsharpModule>();
        this.DependsOn<TypeScriptModule>();
        this.DependencyResolver.Get<GeneratorCommandFactory>().Register<JsonReadCommand>(JsonReadCommandParameters.Names);
        this.DependencyResolver.Get<GeneratorCommandFactory>().Register<JsonWriteCommand>(JsonWriteCommandParameters.Names);
    }

    public override void Initialize()
    {
        this.DependencyResolver.Get<ITypeMapping>().Initialize();
    }
}
