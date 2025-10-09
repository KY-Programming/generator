using KY.Core.Dependency;
using KY.Core.Module;
using KY.Generator.AspDotNet.Commands;
using KY.Generator.Command;

namespace KY.Generator.AspDotNet;

public class AspDotNetModule : ModuleBase
{
    public AspDotNetModule(IDependencyResolver dependencyResolver)
        : base(dependencyResolver)
    {
        this.DependencyResolver.Get<GeneratorCommandFactory>().Register<AspDotNetReadControllerCommand>(AspDotNetReadControllerCommand.Names);
        this.DependencyResolver.Get<GeneratorCommandFactory>().Register<AspDotNetReadHubCommand>(AspDotNetReadHubCommand.Names);
        this.DependencyResolver.Bind<IOptionsFactory>().ToSingleton<AspDotNetOptionsFactory>();
    }
}