using KY.Core.Dependency;
using KY.Core.Module;
using KY.Generator.Configuration;
using KY.Generator.Mappings;
using KY.Generator.Models;
using KY.Generator.Tsql.Extensions;

namespace KY.Generator.Tsql
{
    public class TsqlModule : ModuleBase
    {
        public TsqlModule(IDependencyResolver dependencyResolver)
            : base(dependencyResolver)
        { }

        public override void Initialize()
        {
            this.DependencyResolver.Bind<IGenerator>().To<TsqlGenerator>();
            this.DependencyResolver.Bind<IConfigurationReader>().To<TsqlConfigurationReader>();
            StaticResolver.TypeMapping = this.DependencyResolver.Get<ITypeMapping>().Initialize();
        }
    }
}