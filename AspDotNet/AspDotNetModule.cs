using KY.Core.Dependency;
using KY.Core.Module;
using KY.Generator.AspDotNet.Configurations;
using KY.Generator.AspDotNet.Readers;
using KY.Generator.AspDotNet.Writers;
using KY.Generator.Configuration;

namespace KY.Generator.AspDotNet
{
    public class AspDotNetModule : ModuleBase
    {
        public AspDotNetModule(IDependencyResolver dependencyResolver)
            : base(dependencyResolver)
        { }

        public override void Initialize()
        {
            this.DependencyResolver.Get<ReaderConfigurationMapping>().Map<AspDotNetReadConfiguration, AspDotNetReader>("asp");
            this.DependencyResolver.Get<WriterConfigurationMapping>().Map<AspDotNetWriteConfiguration, AspDotNetWriter>("asp");
        }
    }
}