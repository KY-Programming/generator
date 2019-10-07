using System.Runtime.CompilerServices;
using KY.Core.Dependency;
using KY.Core.Module;
using KY.Generator.Client;
using KY.Generator.Command;
using KY.Generator.Commands;
using KY.Generator.Configuration;
using KY.Generator.Languages;
using KY.Generator.Transfer;

[assembly: InternalsVisibleTo("KY.Generator.Tests")]
[assembly: InternalsVisibleTo("KY.Generator.Core.Tests")]
[assembly: InternalsVisibleTo("KY.Generator.Reflection.Tests")]
[assembly: InternalsVisibleTo("KY.Generator.Csharp.Tests")]
[assembly: InternalsVisibleTo("KY.Generator.Json.Tests")]
[assembly: InternalsVisibleTo("KY.Generator.TypeScript.Tests")]

namespace KY.Generator
{
    internal class CoreModule : ModuleBase
    {
        public CoreModule(IDependencyResolver dependencyResolver)
            : base(dependencyResolver)
        { }

        public override void Initialize()
        {
            this.DependencyResolver.Bind<IGeneratorCommand>().To<ClientCommand>();
            this.DependencyResolver.Bind<IGeneratorCommand>().To<VersionCommand>();
            this.DependencyResolver.Bind<IGeneratorCommand>().To<RunCommand>();
            this.DependencyResolver.Bind<ILanguage>().To<EmptyLanguage>();
            this.DependencyResolver.Get<WriterConfigurationMapping>().Map<GeneratorConfiguration, GeneratorGenerator>("generator");
            this.DependencyResolver.Get<WriterConfigurationMapping>().Map<ModelWriteConfiguration, ModelWriter>("model");
        }
    }
}