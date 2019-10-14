using KY.Core.Dependency;
using KY.Generator.Configuration;
using KY.Generator.Mappings;
using KY.Generator.Module;
using KY.Generator.OData.Configuration;
using KY.Generator.OData.Extensions;
using KY.Generator.OData.Readers;

namespace KY.Generator.OData
{
    public class ODataModule : GeneratorModule
    {
        public ODataModule(IDependencyResolver dependencyResolver)
            : base(dependencyResolver)
        { }

        public override void Initialize()
        {
            this.DependencyResolver.Get<ITypeMapping>().Initialize();
            this.DependencyResolver.Get<ReaderConfigurationMapping>().Map<ODataReadConfiguration, ODataReader>("odata-v4");
        }
    }
}