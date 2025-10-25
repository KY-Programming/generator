using KY.Core.Dependency;
using KY.Generator.Angular.Commands;
using KY.Generator.Angular.Languages;
using KY.Generator.Models;
using KY.Generator.TypeScript;

namespace KY.Generator.Angular;

public class AngularModule : GeneratorModule
{
    public AngularModule(IDependencyResolver dependencyResolver)
        : base(dependencyResolver)
    {
        this.DependsOn<TypeScriptModule>();
        this.Register<AngularServiceCommand>(AngularServiceCommandParameters.Names);
        this.Register<AngularModelCommand>(AngularModelCommandParameters.Names);
        this.Register<AngularPackageCommand>(AngularPackageCommandParameters.Names);
        this.RegisterLanguage<AngularTypeScriptLanguage>();
        this.RegisterOptions<AngularOptionsFactory>();
    }
}
