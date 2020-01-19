using KY.Core.Dependency;
using KY.Core.Module;
using KY.Generator.Command;
using KY.Generator.EntityFramework.Commands;
using KY.Generator.EntityFramework.Configurations;

namespace KY.Generator.EntityFramework
{
    public class EntityFrameworkModule : ModuleBase
    {
        public EntityFrameworkModule(IDependencyResolver dependencyResolver)
            : base(dependencyResolver)
        {
            this.DependencyResolver.Get<CommandRegistry>()
                .Register<WriteEntityFrameworkCommand, EntityFrameworkWriteConfiguration>("ef", "write")
                .Register<WriteEntityFrameworkCommand, EntityFrameworkCoreWriteConfiguration>("ef-core", "write");
        }
    }
}