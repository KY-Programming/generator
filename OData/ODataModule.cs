using KY.Core.Dependency;
using KY.Core.Module;
using KY.Generator.Configuration;
using KY.Generator.Mappings;
using KY.Generator.Models;
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
            this.DependencyResolver.Bind<IConfigurationReader>().To<ODataConfigurationReader>();
            this.DependencyResolver.Bind<IGenerator>().To<ODataGenerator>();
            StaticResolver.TypeMapping = this.DependencyResolver.Get<ITypeMapping>().Initialize();
        }
    }
}