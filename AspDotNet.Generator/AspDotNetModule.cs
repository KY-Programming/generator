using KY.Core.Dependency;
using KY.Generator.Angular;
using KY.Generator.AspDotNet.Commands;
using KY.Generator.AspDotNet.Fluent;
using KY.Generator.Csharp;
using KY.Generator.Models;
using KY.Generator.Reflection;

namespace KY.Generator.AspDotNet;

public class AspDotNetModule : GeneratorModule
{
    public AspDotNetModule(IDependencyResolver dependencyResolver)
        : base(dependencyResolver)
    {
        this.DependsOn<AngularModule>();
        this.DependsOn<CsharpModule>();
        this.DependsOn<ReflectionModule>();
        this.Register<AspDotNetReadControllerCommand>(AspDotNetReadControllerCommandParameters.Names);
        this.Register<AspDotNetReadHubCommand>(AspDotNetReadHubCommandParameters.Names);
        this.Register<IAspDotNetReadSyntax, AspDotNetReadSyntax>();
        this.RegisterOptions<AspDotNetOptionsFactory>();
    }
}
