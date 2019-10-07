using KY.Core.Dependency;
using KY.Core.Module;
using KY.Generator.Angular.Commands;
using KY.Generator.Angular.Configurations;
using KY.Generator.Angular.Writers;
using KY.Generator.Command;
using KY.Generator.Configuration;
using KY.Generator.Transfer;

namespace KY.Generator.Angular
{
    public class AngularModule : ModuleBase
    {
        public AngularModule(IDependencyResolver dependencyResolver)
            : base(dependencyResolver)
        { }

        public override void Initialize()
        {
            this.DependencyResolver.Bind<IGeneratorCommand>().To<GenerateAngularConfig>();
            this.DependencyResolver.Get<WriterConfigurationMapping>().Map<AngularConfiguration, AngularWriter>("angular");
        }
    }
}