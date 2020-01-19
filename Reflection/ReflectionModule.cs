using KY.Core.Dependency;
using KY.Core.Module;
using KY.Generator.Command;
using KY.Generator.Mappings;
using KY.Generator.Reflection.Commands;
using KY.Generator.Reflection.Configurations;
using KY.Generator.Reflection.Extensions;
using KY.Generator.Reflection.Readers;

namespace KY.Generator.Reflection
{
    public class ReflectionModule : ModuleBase
    {
        public ReflectionModule(IDependencyResolver dependencyResolver)
            : base(dependencyResolver)
        { }

        public override void Initialize()
        {
            this.DependencyResolver.Get<ITypeMapping>().Initialize();
            this.DependencyResolver.Bind<ReflectionModelReader>().ToSelf();
            this.DependencyResolver.Bind<ReadReflectionCommand>().ToSelf();
            this.DependencyResolver.Bind<WriteReflectionCommand>().ToSelf();
            this.DependencyResolver.Get<CommandRegistry>()
                .Register<ReflectionCommand, ReflectionConfiguration>("reflection")
                .Register<ReadReflectionCommand, ReadReflectionConfiguration>("reflection", "read")
                .Register<WriteReflectionCommand, WriteReflectionConfiguration>("reflection", "write");
        }
    }
}