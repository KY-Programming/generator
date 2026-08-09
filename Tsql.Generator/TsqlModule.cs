using KY.Core.Dependency;
using KY.Generator.Csharp;
using KY.Generator.Mappings;
using KY.Generator.Models;
using KY.Generator.Tsql.Commands;
using KY.Generator.Tsql.Fluent;
using KY.Generator.Tsql.Language;
using KY.Generator.TypeScript;

namespace KY.Generator.Tsql;

public class TsqlModule : GeneratorModule
{
    public TsqlModule(IDependencyResolver dependencyResolver)
        : base(dependencyResolver)
    {
        this.DependsOn<CsharpModule>();
        this.DependsOn<TypeScriptModule>();
        this.Register<TsqlReadCommand>(TsqlReadCommandParameters.Names);
        this.Register<ITsqlReadSyntax, TsqlReadSyntax>();
    }

    public override void Initialize()
    {
        this.DependencyResolver.Get<ITypeMapping>().Initialize();
    }
}
