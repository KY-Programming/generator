using System.Runtime.CompilerServices;
using KY.Core.Dependency;
using KY.Generator.Commands;
using KY.Generator.Languages;
using KY.Generator.Models;

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

internal class CoreModule : GeneratorModule
{
    public CoreModule(IDependencyResolver dependencyResolver)
        : base(dependencyResolver)
    {
        this.Register<VersionCommand>(VersionCommand.Names);
        this.Register<ReadProjectCommand>(ReadProjectCommand.Names);
        this.Register<StatisticsCommand>(StatisticsCommand.Names);
        this.Register<OptionsCommand>(OptionsCommand.Names);
        this.Register<CleanupCommand>(CleanupCommand.Names);
        this.Register<GetLicenseCommand>(GetLicenseCommand.Names);
        this.Register<FluentCommand>(FluentCommand.Names);
        this.Register<AnnotationCommand>(AnnotationCommand.Names);
        this.Register<LoadCommand>(LoadCommand.Names);
        this.Register<ForceCommand>(ForceCommand.Names);
        this.Register<MsBuildCommand>(MsBuildCommand.Names);
        this.Register<BeforeBuildCommand>(BeforeBuildCommand.Names);
        this.Register<NoHeaderCommand>(NoHeaderCommand.Names);
        this.RegisterLanguage<EmptyLanguage>();
        this.RegisterOptions<GeneratorOptionsFactory>();
        this.DependencyResolver.Bind<GeneratorTypeLoader>().ToSingleton();
        // Register the options factories. This is not needed for other modules because the dependency resolver keeps the list for all modules.
        Options.Register(() => this.DependencyResolver.Get<List<IOptionsFactory>>());
    }
}
