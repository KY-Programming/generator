using KY.Core.Dependency;
using KY.Generator.Csharp.Languages;
using KY.Generator.Models;

namespace KY.Generator.Csharp;

public class CsharpModule : GeneratorModule
{
    public CsharpModule(IDependencyResolver dependencyResolver)
        : base(dependencyResolver)
    {
        this.RegisterLanguage<CsharpLanguage>();
    }
}
