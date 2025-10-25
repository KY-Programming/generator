using KY.Core.Dependency;
using KY.Core.Module;
using KY.Generator.Mappings;
using KY.Generator.OData.Extensions;

namespace KY.Generator.OData
{
    public class ODataModule : ModuleBase
    {
        public ODataModule(IDependencyResolver dependencyResolver)
            : base(dependencyResolver)
        { }

        public override void Initialize()
        {
            this.DependencyResolver.Get<ITypeMapping>().Initialize();
        }
    }
}
