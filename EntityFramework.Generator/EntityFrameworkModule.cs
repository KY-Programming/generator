using KY.Core.Dependency;
using KY.Generator.Csharp;
using KY.Generator.Models;
using KY.Generator.Tsql;

namespace KY.Generator.EntityFramework;

public class EntityFrameworkModule : GeneratorModule
{
    public EntityFrameworkModule(IDependencyResolver dependencyResolver)
        : base(dependencyResolver)
    {
        this.DependsOn<CsharpModule>();
        this.DependsOn<TsqlModule>();
    }
}
