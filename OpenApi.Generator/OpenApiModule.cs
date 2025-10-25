using KY.Core.Dependency;
using KY.Generator.Csharp;
using KY.Generator.Mappings;
using KY.Generator.Models;
using KY.Generator.OpenApi.DataAccess;
using KY.Generator.OpenApi.Languages;
using KY.Generator.OpenApi.Readers;
using KY.Generator.TypeScript;

namespace KY.Generator.OpenApi;

public class OpenApiModule : GeneratorModule
{
    public OpenApiModule(IDependencyResolver dependencyResolver)
        : base(dependencyResolver)
    {
        this.DependsOn<CsharpModule>();
        this.DependsOn<TypeScriptModule>();
        this.DependencyResolver.Bind<OpenApiDocumentReader>().ToSelf();
        this.DependencyResolver.Bind<OpenApiFileReader>().ToSelf();
        this.DependencyResolver.Bind<OpenApiUrlReader>().ToSelf();
    }

    public override void Initialize()
    {
        this.DependencyResolver.Get<ITypeMapping>().Initialize();
    }
}
