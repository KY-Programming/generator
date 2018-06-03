using KY.Core.Dependency;
using KY.Core.Module;

namespace KY.Generator.Module
{
    public abstract class GeneratorModule : ModuleBase
    {
        protected GeneratorModule(IDependencyResolver dependencyResolver)
            : base(dependencyResolver)
        { }

        public virtual void BeforeConfigure()
        { }

        public virtual void BeforeRun()
        { }

        public virtual void AfterRun()
        { }
    }
}