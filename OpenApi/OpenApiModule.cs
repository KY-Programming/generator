using KY.Core.Dependency;
using KY.Core.Module;
using KY.Generator.Command;
using KY.Generator.Mappings;
using KY.Generator.OpenApi.Commands;
using KY.Generator.OpenApi.Configurations;
using KY.Generator.OpenApi.DataAccess;
using KY.Generator.OpenApi.Languages;
using KY.Generator.OpenApi.Readers;

namespace KY.Generator.OpenApi
{
    public class OpenApiModule : ModuleBase
    {
        public OpenApiModule(IDependencyResolver dependencyResolver)
            : base(dependencyResolver)
        { }

        public override void Initialize()
        {
            this.DependencyResolver.Bind<OpenApiDocumentReader>().ToSelf();
            this.DependencyResolver.Bind<OpenApiFileReader>().ToSelf();
            this.DependencyResolver.Bind<OpenApiUrlReader>().ToSelf();
            this.DependencyResolver.Get<CommandRegistry>()
                .Register<ReadOpenApiCommand, OpenApiReadConfiguration>("openApi", "read");
            this.DependencyResolver.Get<ITypeMapping>().Initialize();
        }
    }
}