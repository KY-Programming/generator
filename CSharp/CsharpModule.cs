using KY.Core.Dependency;
using KY.Core.Module;
using KY.Generator.Csharp.Languages;
using KY.Generator.Languages;

namespace KY.Generator.Csharp
{
    public class CsharpModule : ModuleBase
    {
        public CsharpModule(IDependencyResolver dependencyResolver)
            : base(dependencyResolver)
        { }

        public override void Initialize()
        {
            this.DependencyResolver.Bind<ILanguage>().To(CsharpLanguage.Instance);
        }
    }
}