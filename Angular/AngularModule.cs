using KY.Core.Dependency;
using KY.Core.Module;
using KY.Generator.Angular.Commands;
using KY.Generator.Angular.Languages;
using KY.Generator.Command;
using KY.Generator.Languages;

namespace KY.Generator.Angular;

public class AngularModule : ModuleBase
{
    public AngularModule(IDependencyResolver dependencyResolver)
        : base(dependencyResolver)
    {
        this.DependencyResolver.Get<GeneratorCommandFactory>().Register<AngularServiceCommand>(AngularServiceCommand.Names);
        this.DependencyResolver.Get<GeneratorCommandFactory>().Register<AngularModelCommand>(AngularModelCommand.Names);
        this.DependencyResolver.Get<GeneratorCommandFactory>().Register<AngularPackageCommand>(AngularPackageCommand.Names);
        this.DependencyResolver.Bind<ILanguage>().To<AngularTypeScriptLanguage>();
        this.DependencyResolver.Bind<IOptionsFactory>().ToSingleton<AngularOptionsFactory>();
    }
}