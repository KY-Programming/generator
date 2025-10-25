using System.Reflection;
using KY.Core;
using KY.Core.Dependency;
using KY.Generator.Command;
using KY.Generator.Extensions;
using KY.Generator.Helpers;
using KY.Generator.Models;
using KY.Generator.Syntax;
using KY.Generator.Transfer;

namespace KY.Generator.Commands;

internal class FluentCommand : GeneratorCommand<FluentCommandParameters>
{
    private readonly IDependencyResolver resolver;
    private readonly List<GeneratorFluentMain> mains = new();

    public FluentCommand(IDependencyResolver resolver)
    {
        this.resolver = resolver;
    }

    public override IGeneratorCommandResult Run()
    {
        IEnvironment environment = this.resolver.Get<IEnvironment>();
        if (environment.LoadedAssemblies.Count == 0)
        {
            if (environment.IsBeforeBuild)
            {
                return this.Success();
            }
            Logger.Error($"Can not run '{this.Parameters.CommandName}' command without loaded assemblies. Add at least one 'load -assembly=<assembly-path>' command before.");
            return this.Error();
        }
        foreach (Assembly assembly in environment.LoadedAssemblies)
        {
            if (!this.Parameters.IsOnlyAsync && assembly.IsAsync())
            {
                return this.SwitchAsync();
            }
            IEnumerable<Type> types = TypeHelper.GetTypes(assembly).Where(type => typeof(GeneratorFluentMain).IsAssignableFrom(type));
            foreach (Type objectType in types)
            {
                GeneratorFluentMain main = (GeneratorFluentMain)this.resolver.Create(objectType);
                this.mains.Add(main);
                main.Resolver = this.resolver;
                if (environment.IsBeforeBuild)
                {
                    main.ExecuteBeforeBuild();
                }
                else
                {
                    main.Execute();
                }
                foreach (IFluentInternalSyntax syntax in main.Syntaxes)
                {
                    IGeneratorCommandResult commandResult = syntax.Run();
                    if (!commandResult.Success)
                    {
                        return commandResult;
                    }
                    environment.TransferObjects.AddIfNotExists(syntax.Resolver.Get<List<ITransferObject>>());
                }
            }
        }
        return this.Success();
    }

    public override void FollowUp()
    {
        base.FollowUp();
        this.mains.ForEach(main => main.Syntaxes.ForEach(syntax => syntax.FollowUp()));
    }
}
