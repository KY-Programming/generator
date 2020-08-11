using KY.Core.Dependency;
using KY.Core.Module;
using KY.Generator.AspDotNet.Commands;
using KY.Generator.AspDotNet.Configurations;
using KY.Generator.AspDotNet.Readers;
using KY.Generator.AspDotNet.Writers;
using KY.Generator.Command;
using KY.Generator.Configuration;

namespace KY.Generator.AspDotNet
{
    public class AspDotNetModule : ModuleBase
    {
        public AspDotNetModule(IDependencyResolver dependencyResolver)
            : base(dependencyResolver)
        {
            this.DependencyResolver.Bind<IGeneratorCommand>().To<AspDotNetReadControllerCommand>();
        }

        public override void Initialize()
        {
            this.DependencyResolver.Get<ConfigurationMapping>()
                .Map<AspDotNetReadConfiguration, AspDotNetReader>("asp")
                .Map<AspDotNetWriteConfiguration, AspDotNetWriter>("asp")
                .Map<AspDotNetCoreWriteConfiguration, AspDotNetWriter>("asp-core");
        }
    }
}