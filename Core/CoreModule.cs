using System.Runtime.CompilerServices;
using KY.Core.Dependency;
using KY.Core.Module;
using KY.Generator.Command;
using KY.Generator.Commands;
using KY.Generator.Configurations;
using KY.Generator.Languages;
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
            this.DependencyResolver.Bind<ILanguage>().To<EmptyLanguage>();
            this.DependencyResolver.Bind<ModelWriter>().ToSelf();
            this.DependencyResolver.Get<CommandRegistry>()
                //.Register<ClientCommand, ClientConfiguration>("client")
                .Register<VersionCommand, VersionConfiguration>("version").Alias("v")
                .Register<ExecuteCommand, ExecuteConfiguration>("execute").Alias("exec", "e").Alias("run", "r")
                .Register<ExecuteSeparateCommand, ExecuteSeparateConfiguration>("separate")
                .Register<ReadCookieCommand, CookieConfiguration>("cookie", "read")
                //.Register<GeneratorGenerator, GeneratorConfiguration>("generator", "write")
                .Register<ModelWriteCommand, ModelWriteConfiguration>("model", "write")
                .Register<DemoCommand, DemoConfiguration>("demo");
        }
    }
}