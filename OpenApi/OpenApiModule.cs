﻿using KY.Core.Dependency;
using KY.Core.Module;
using KY.Generator.Configuration;
using KY.Generator.Mappings;
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
        {
            this.DependencyResolver.Bind<OpenApiDocumentReader>().ToSelf();
            this.DependencyResolver.Bind<OpenApiFileReader>().ToSelf();
            this.DependencyResolver.Bind<OpenApiUrlReader>().ToSelf();
        }

        public override void Initialize()
        {
            this.DependencyResolver.Get<ConfigurationMapping>().Map<OpenApiReadConfiguration, OpenApiReader>("openApi");
            this.DependencyResolver.Get<ITypeMapping>().Initialize();
        }
    }
}