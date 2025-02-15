using System.Collections.Generic;
using System.Runtime.CompilerServices;
using KY.Core.Dependency;
using KY.Core.Module;
using KY.Generator.Command;
using KY.Generator.Commands;
using KY.Generator.Languages;

[assembly: InternalsVisibleTo("KY.Generator.Tests")]
[assembly: InternalsVisibleTo("KY.Generator.Common.Tests")]
[assembly: InternalsVisibleTo("KY.Generator.Reflection.Tests")]
[assembly: InternalsVisibleTo("KY.Generator.Csharp.Tests")]
[assembly: InternalsVisibleTo("KY.Generator.Json.Tests")]
[assembly: InternalsVisibleTo("KY.Generator.TypeScript.Tests")]
[assembly: InternalsVisibleTo("KY.Generator.AspDotNet.Tests")]
[assembly: InternalsVisibleTo("KY.Generator.OData.Tests")]
[assembly: InternalsVisibleTo("KY.Generator.Tsql.Tests")]
[assembly: InternalsVisibleTo("KY.Generator.Angular.Tests")]

namespace KY.Generator;

internal class CoreModule : ModuleBase
{
    public CoreModule(IDependencyResolver dependencyResolver)
        : base(dependencyResolver)
    {
        this.DependencyResolver.Get<GeneratorCommandFactory>().Register<VersionCommand>(VersionCommand.Names);
        this.DependencyResolver.Get<GeneratorCommandFactory>().Register<ReadProjectCommand>(ReadProjectCommand.Names);
        this.DependencyResolver.Get<GeneratorCommandFactory>().Register<StatisticsCommand>(StatisticsCommand.Names);
        this.DependencyResolver.Get<GeneratorCommandFactory>().Register<OptionsCommand>(OptionsCommand.Names);
        this.DependencyResolver.Get<GeneratorCommandFactory>().Register<CleanupCommand>(CleanupCommand.Names);
        this.DependencyResolver.Get<GeneratorCommandFactory>().Register<GetLicenseCommand>(GetLicenseCommand.Names);
        this.DependencyResolver.Bind<ILanguage>().To<EmptyLanguage>();
        this.DependencyResolver.Bind<IOptionsFactory>().ToSingleton<GeneratorOptionsFactory>();
        Options.Register(() => this.DependencyResolver.Get<List<IOptionsFactory>>());
    }
}
