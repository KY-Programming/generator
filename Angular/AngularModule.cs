using KY.Core.Dependency;
using KY.Core.Module;
using KY.Generator.Angular.Commands;
using KY.Generator.Angular.Languages;
using KY.Generator.Command;
using KY.Generator.Languages;

namespace KY.Generator.Angular
{
    public class AngularModule : ModuleBase
    {
        public AngularModule(IDependencyResolver dependencyResolver)
            : base(dependencyResolver)
        {
            this.DependencyResolver.Bind<IGeneratorCommand>().To<AngularServiceCommand>();
            this.DependencyResolver.Bind<IGeneratorCommand>().To<AngularModelCommand>();
            this.DependencyResolver.Bind<ILanguage>().To<AngularTypeScriptLanguage>();
        }
    }
}
