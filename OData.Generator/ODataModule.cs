using KY.Core.Dependency;
using KY.Generator.Mappings;
using KY.Generator.Models;
using KY.Generator.OData.Extensions;
using KY.Generator.TypeScript;

namespace KY.Generator.OData;

public class ODataModule : GeneratorModule
{
    public ODataModule(IDependencyResolver dependencyResolver)
        : base(dependencyResolver)
    {
        this.DependsOn<TypeScriptModule>();
    }

    public override void Initialize()
    {
        this.DependencyResolver.Get<ITypeMapping>().Initialize();
    }
}
