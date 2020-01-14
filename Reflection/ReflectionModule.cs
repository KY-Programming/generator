using KY.Core.Dependency;
using KY.Core.Module;
using KY.Generator.Command;
using KY.Generator.Configuration;
using KY.Generator.Mappings;
using KY.Generator.Reflection.Commands;
using KY.Generator.Reflection.Configurations;
using KY.Generator.Reflection.Extensions;
using KY.Generator.Reflection.Readers;
using KY.Generator.Reflection.Writers;

namespace KY.Generator.Reflection
{
    public class ReflectionModule : ModuleBase
    {
        public ReflectionModule(IDependencyResolver dependencyResolver)
            : base(dependencyResolver)
        { }

        public override void Initialize()
        {
            this.DependencyResolver.Bind<IGeneratorCommand>().To<ReflectionCommand>();
            this.DependencyResolver.Get<ITypeMapping>().Initialize();
            this.DependencyResolver.Bind<ReflectionModelReader>().ToSelf();
            this.DependencyResolver.Bind<ReflectionReader>().ToSelf();
            this.DependencyResolver.Bind<ReflectionWriter>().ToSelf();
            this.DependencyResolver.Get<ConfigurationMapping>()
                .Map<ReflectionReadConfiguration, ReflectionReader>("reflection")
                .Map<ReflectionWriteConfiguration, ReflectionWriter>("reflection");
        }
    }
}