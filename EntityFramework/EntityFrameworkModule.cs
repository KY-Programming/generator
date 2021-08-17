using KY.Core.Dependency;
using KY.Core.Module;

namespace KY.Generator.EntityFramework
{
    public class EntityFrameworkModule : ModuleBase
    {
        public EntityFrameworkModule(IDependencyResolver dependencyResolver)
            : base(dependencyResolver)
        { }
    }
}
