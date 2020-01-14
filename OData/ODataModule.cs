using KY.Core.Dependency;
using KY.Core.Module;
using KY.Generator.Configuration;
using KY.Generator.Mappings;
using KY.Generator.OData.Configurations;
using KY.Generator.OData.Extensions;
using KY.Generator.OData.Readers;

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
            this.DependencyResolver.Get<ConfigurationMapping>().Map<ODataReadConfiguration, ODataReader>("odata-v4");
        }
    }
}