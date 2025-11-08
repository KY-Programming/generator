using KY.Core;
using KY.Core.Dependency;
using KY.Core.Module;
using KY.Generator.Command;
using KY.Generator.Languages;

namespace KY.Generator.Models;

public abstract class GeneratorModule : ModuleBase
{
    protected GeneratorModule(IDependencyResolver dependencyResolver)
        : base(dependencyResolver)
    { }

    protected void DependsOn<T>()
        where T : ModuleBase
    {
        // Ensures that the module is loaded
        this.Log(() => this.DependencyResolver.Get<GeneratorModuleLoader>().Load(typeof(T).Assembly));
    }

    protected void Register<T>(IEnumerable<string> names) where T : IGeneratorCommand
    {
        this.Log(() => this.DependencyResolver.Get<GeneratorCommandFactory>().Register<T>(names));
    }

    protected void Register(Type command, IEnumerable<string> names)
    {
        this.Log(() => this.DependencyResolver.Get<GeneratorCommandFactory>().Register(command, names));
    }

    protected void Register<TInterface, TSyntax>()
        where TInterface : IFluentSyntax<TInterface>
        where TSyntax : TInterface
    {
        this.DependencyResolver.Get<ISyntaxResolver>().Register<TInterface, TSyntax>();
    }

    protected void RegisterLanguage<T>() where T : ILanguage
    {
        this.Log(() => this.DependencyResolver.Bind<ILanguage>().To<T>());
    }

    protected void RegisterOptions<T>() where T : IOptionsFactory
    {
        this.Log(() => this.DependencyResolver.Bind<IOptionsFactory>().ToSingleton<T>());
    }

    // TODO: This method is currently needed to log exceptions, because ModuleBase swallows them
    private void Log(Action action)
    {
        try
        {
            action();
        }
        catch (Exception exception)
        {
            Logger.Error(exception);
            throw;
        }
    }
}
