using KY.Core.Dependency;
using KY.Generator.Csharp;
using KY.Generator.Mappings;
using KY.Generator.Models;
using KY.Generator.Reflection.Commands;
using KY.Generator.Reflection.Extensions;
using KY.Generator.Reflection.Readers;
using KY.Generator.Reflection.Writers;
using KY.Generator.TypeScript;

namespace KY.Generator.Reflection;

public class ReflectionModule : GeneratorModule
{
    public ReflectionModule(IDependencyResolver dependencyResolver)
        : base(dependencyResolver)
    {
        this.DependsOn<CsharpModule>();
        this.DependsOn<TypeScriptModule>();
        this.DependencyResolver.Bind<ReflectionModelReader>().ToSelf();
        this.DependencyResolver.Bind<ReflectionReader>().ToSelf();
        this.DependencyResolver.Bind<ReflectionWriter>().ToSelf();
        this.Register<ReflectionCommand>(ReflectionCommandParameters.Names);
        this.Register<ReflectionReadCommand>(ReflectionReadCommandParameters.Names);
        this.Register<ReflectionWriteCommand>(ReflectionWriteCommandParameters.Names);
    }

    public override void Initialize()
    {
        //this.DependencyResolver.Bind<ReflectionGeneratorConfiguration>().ToSingleton();
        this.DependencyResolver.Get<ITypeMapping>().Initialize();
        //StaticResolver.GeneratorConfiguration = this.DependencyResolver.Get<ReflectionGeneratorConfiguration>();
    }
}
