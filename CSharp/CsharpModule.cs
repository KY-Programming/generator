﻿using KY.Core.Dependency;
using KY.Core.Module;
using KY.Generator.Languages;

namespace KY.Generator
{
    public class CsharpModule : ModuleBase
    {
        public CsharpModule(IDependencyResolver dependencyResolver)
            : base(dependencyResolver)
        { }

        public override void Initialize()
        {
            this.DependencyResolver.Bind<ILanguage>().ToConstant(Csharp.Language);
        }
    }
}