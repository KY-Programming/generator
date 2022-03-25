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

namespace KY.Generator
{
    internal class CoreModule : ModuleBase
    {
        public CoreModule(IDependencyResolver dependencyResolver)
            : base(dependencyResolver)
        {
            //this.DependencyResolver.Bind<IGeneratorCommand>().To<ClientCommand>();
            this.DependencyResolver.Bind<IGeneratorCommand>().To<VersionCommand>();
            this.DependencyResolver.Bind<IGeneratorCommand>().To<ReadIdCommand>();
            this.DependencyResolver.Bind<IGeneratorCommand>().To<StatisticsCommand>();
            this.DependencyResolver.Bind<IGeneratorCommand>().To<OptionsCommand>();
            this.DependencyResolver.Bind<IGeneratorCommand>().To<CleanupCommand>();
            this.DependencyResolver.Bind<IGeneratorCommand>().To<GetLicenseCommand>();
            this.DependencyResolver.Bind<ILanguage>().To<EmptyLanguage>();
        }
    }
}
