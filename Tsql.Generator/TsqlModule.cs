using KY.Core.Dependency;
using KY.Core.Module;
using KY.Generator.Command;
using KY.Generator.Mappings;
using KY.Generator.Tsql.Commands;
using KY.Generator.Tsql.Language;

namespace KY.Generator.Tsql;

public class TsqlModule : ModuleBase
{
    public TsqlModule(IDependencyResolver dependencyResolver)
        : base(dependencyResolver)
    {
        this.DependencyResolver.Get<GeneratorCommandFactory>().Register<TsqlReadCommand>(TsqlReadCommandParameters.Names);
    }

    public override void Initialize()
    {
        this.DependencyResolver.Get<ITypeMapping>().Initialize();
    }
}
