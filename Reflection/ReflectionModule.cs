using KY.Core.Dependency;
using KY.Core.Module;
using KY.Generator.Command;
using KY.Generator.Mappings;
using KY.Generator.Reflection.Commands;
using KY.Generator.Reflection.Extensions;
using KY.Generator.Reflection.Readers;
using KY.Generator.Reflection.Writers;

namespace KY.Generator.Reflection
{
    public class ReflectionModule : ModuleBase
    {
        public ReflectionModule(IDependencyResolver dependencyResolver)
            : base(dependencyResolver)
        {
            this.DependencyResolver.Bind<ReflectionModelReader>().ToSelf();
            this.DependencyResolver.Bind<ReflectionReader>().ToSelf();
            this.DependencyResolver.Bind<ReflectionWriter>().ToSelf();
            this.DependencyResolver.Bind<IGeneratorCommand>().To<ReflectionCommand>();
            this.DependencyResolver.Bind<IGeneratorCommand>().To<AnnotationCommand>();
            this.DependencyResolver.Bind<IGeneratorCommand>().To<ReflectionReadCommand>();
            this.DependencyResolver.Bind<IGlobalOptionsReader>().To<ReflectionOptionsReader>();
        }

        public override void Initialize()
        {
            //this.DependencyResolver.Bind<ReflectionGeneratorConfiguration>().ToSingleton();
            this.DependencyResolver.Get<ITypeMapping>().Initialize();
            //StaticResolver.GeneratorConfiguration = this.DependencyResolver.Get<ReflectionGeneratorConfiguration>();
        }
    }
}
