using CreateModule.Configurations;
using CreateModule.Readers;
using CreateModule.Writers;
using KY.Core.Dependency;
using KY.Core.Module;
using KY.Generator.Configuration;

namespace CreateModule
{
    public class DemoModule : ModuleBase
    {
        public DemoModule(IDependencyResolver dependencyResolver)
            : base(dependencyResolver)
        { }

        public override void Initialize()
        {
            base.Initialize();
            this.DependencyResolver.Get<ConfigurationMapping>()
                .Map<DemoReadConfiguration, DemoReader>("my-demo")
                .Map<DemoWriteConfiguration, DemoWriter>("my-demo");
        }
    }
}