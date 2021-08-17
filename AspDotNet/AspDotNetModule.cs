using KY.Core.Dependency;
using KY.Core.Module;
using KY.Generator.AspDotNet.Commands;
using KY.Generator.Command;

namespace KY.Generator.AspDotNet
{
    public class AspDotNetModule : ModuleBase
    {
        public AspDotNetModule(IDependencyResolver dependencyResolver)
            : base(dependencyResolver)
        {
            this.DependencyResolver.Bind<IGeneratorCommand>().To<AspDotNetReadControllerCommand>();
            this.DependencyResolver.Bind<IGeneratorCommand>().To<AspDotNetReadHubCommand>();
            Options.Register<AspDotNetOptions>();
            this.DependencyResolver.Bind<AspDotNetOptionsReader>().ToSelf();
        }
    }
}
