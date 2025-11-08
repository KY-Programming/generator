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
        this.DependencyResolver.Bind<ISyntaxResolver>().ToSingleton<SyntaxResolver>();
        this.DependencyResolver.Bind<GeneratorTypeLoader>().ToSingleton();
        this.Register<VersionCommand>(VersionCommandParameters.Names);
        this.Register<ReadProjectCommand>(ReadProjectCommandParameters.Names);
        this.Register<StatisticsCommand>(StatisticsCommandParameters.Names);
        this.Register<OptionsCommand>(OptionsCommandParameters.Names);
        this.Register<CleanupCommand>(CleanupCommandParameters.Names);
        this.Register<GetLicenseCommand>(GetLicenseCommandParameters.Names);
        this.Register<FluentCommand>(FluentCommandParameters.Names);
        this.Register<AnnotationCommand>(AnnotationCommandParameters.Names);
        this.Register<LoadCommand>(LoadCommandParameters.Names);
        this.Register<ForceCommand>(ForceCommandParameters.Names);
        this.Register<MsBuildCommand>(MsBuildCommandParameters.Names);
        this.Register<BeforeBuildCommand>(BeforeBuildCommandParameters.Names);
        this.Register<NoHeaderCommand>(NoHeaderCommandParameters.Names);
        this.Register<ISwitchToReadFluentSyntax, FluentSyntax>();
        this.RegisterLanguage<EmptyLanguage>();
        this.RegisterOptions<GeneratorOptionsFactory>();
        // Register the options factories. This is not needed for other modules because the dependency resolver keeps the list for all modules.
        Options.Register(() => this.DependencyResolver.Get<List<IOptionsFactory>>());
    }
}
