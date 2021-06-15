﻿using KY.Core.Dependency;
using KY.Core.Module;
using KY.Generator.Configuration;
using KY.Generator.Mappings;
using KY.Generator.Tsql.Configurations;
using KY.Generator.Tsql.Language;
using KY.Generator.Tsql.Readers;

namespace KY.Generator.Tsql
{
    public class TsqlModule : ModuleBase
    {
        public TsqlModule(IDependencyResolver dependencyResolver)
            : base(dependencyResolver)
        { }

        public override void Initialize()
        {
            this.DependencyResolver.Get<ITypeMapping>().Initialize();
            this.DependencyResolver.Get<ConfigurationMapping>().Map<TsqlReadConfiguration, TsqlReader>("tsql");
        }
    }
}