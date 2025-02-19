﻿using System;
using System.Collections.Generic;
using KY.Core;
using KY.Core.Dependency;
using KY.Generator.Csharp.Languages;
using KY.Generator.EntityFramework.Configurations;
using KY.Generator.Templates;
using KY.Generator.Transfer.Writers;

namespace KY.Generator.EntityFramework.Writers;

public class EntityFrameworkWriter : ITransferWriter
{
    private readonly IDependencyResolver resolver;
    private readonly Options options;

    public EntityFrameworkWriter(IDependencyResolver resolver, Options options)
    {
        this.resolver = resolver;
        this.options = options;
    }

    public virtual void Write(EntityFrameworkWriteConfiguration configuration)
    {
        if (!this.options.Get<GeneratorOptions>().Language.IsCsharp())
        {
            throw new InvalidOperationException("EntityFramework support only Csharp");
        }
        configuration.Namespace.AssertIsNotNull(nameof(configuration.Namespace), "ef and ef-core writer requires a namespace");

        List<FileTemplate> files = new();
        if (configuration.Repositories.Count > 0)
        {
            this.resolver.Create<EntityFrameworkRepositoryWriter>().Write(configuration);
        }
        this.resolver.Create<EntityFrameworkDataContextWriter>().Write(configuration);
    }
}