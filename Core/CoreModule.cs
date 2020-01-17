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
        { }

        public override void Initialize()
        {
            this.DependencyResolver.Get<CommandRegister>()
                .Register<ClientCommand, ClientConfiguration>("client")
                .Register<VersionCommand, VersionConfiguration>("version")
                .Register<VersionCommand, VersionConfiguration>("v")
                .Register<ExecuteCommand, ExecuteConfiguration>("execute")
                .Register<ExecuteCommand, ExecuteConfiguration>("exec")
                .Register<ExecuteCommand, ExecuteConfiguration>("e")
                .Register<ExecuteCommand, ExecuteConfiguration>("run")
                .Register<ExecuteCommand, ExecuteConfiguration>("r");
            this.DependencyResolver.Bind<IGeneratorCommand>().To<VersionCommand>();
            this.DependencyResolver.Bind<IGeneratorCommand>().To<ExecuteCommand>();
            this.DependencyResolver.Bind<ILanguage>().To<EmptyLanguage>();
            this.DependencyResolver.Get<ConfigurationMapping>()
                .Map<CookieConfiguration, CookieReader>("cookie")
                .Map<GeneratorConfiguration, GeneratorGenerator>("generator")
                .Map<ModelWriteConfiguration, ModelWriter>("model")
                .Map<DemoConfiguration, DemoWriter>("demo")
                .Map<ExecuteConfiguration, ExecuteReader>("generator", "execute");
        }
    }
}