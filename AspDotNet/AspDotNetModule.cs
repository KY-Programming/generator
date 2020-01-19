using KY.Core.Dependency;
using KY.Core.Module;
using KY.Generator.AspDotNet.Commands;
using KY.Generator.AspDotNet.Configurations;
using KY.Generator.Command;

namespace KY.Generator.AspDotNet
{
    public class AspDotNetModule : ModuleBase
    {
        public AspDotNetModule(IDependencyResolver dependencyResolver)
            : base(dependencyResolver)
        { }

        public override void Initialize()
        {
            this.DependencyResolver.Get<CommandRegistry>()
                .Register<ReadAspDotNetCommand, AspDotNetReadConfiguration>("asp", "read")
                .Register<WriteAspDotNetCommand, AspDotNetWriteConfiguration>("asp", "write")
                .Register<WriteAspDotNetCommand, AspDotNetCoreWriteConfiguration>("asp-core", "write");
        }
    }
}