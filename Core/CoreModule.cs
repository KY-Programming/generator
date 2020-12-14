using System.Runtime.CompilerServices;
using KY.Core.Dependency;
using KY.Core.Module;
using KY.Generator.Client;
using KY.Generator.Command;
using KY.Generator.Commands;
using KY.Generator.Configuration;
using KY.Generator.Configurations;
using KY.Generator.Languages;
using KY.Generator.Transfer.Readers;
using KY.Generator.Transfer.Writers;

[assembly: InternalsVisibleTo("KY.Generator.Tests")]
[assembly: InternalsVisibleTo("KY.Generator.Core.Tests")]
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
            this.DependencyResolver.Bind<ILanguage>().To<EmptyLanguage>();
        }

        public override void Initialize()
        {
            this.DependencyResolver.Get<ConfigurationMapping>()
                .Map<CookieConfiguration, CookieReader>("cookie")
                .Map<GeneratorConfiguration, GeneratorGenerator>("generator")
                .Map<ModelWriteConfiguration, ModelWriter>("model");
        }
    }
}