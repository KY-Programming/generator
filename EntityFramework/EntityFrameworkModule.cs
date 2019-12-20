using KY.Core.Dependency;
using KY.Core.Module;
using KY.Generator.Configuration;
using KY.Generator.EntityFramework.Configurations;
using KY.Generator.EntityFramework.Writers;

namespace KY.Generator.EntityFramework
{
    public class EntityFrameworkModule : ModuleBase
    {
        public EntityFrameworkModule(IDependencyResolver dependencyResolver)
            : base(dependencyResolver)
        {
            this.DependencyResolver.Get<ConfigurationMapping>()
                .Map<EntityFrameworkWriteConfiguration, EntityFrameworkWriter>("ef")
                .Map<EntityFrameworkCoreWriteConfiguration, EntityFrameworkWriter>("ef-core");
        }
    }
}