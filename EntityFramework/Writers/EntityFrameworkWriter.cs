using System;
using System.Collections.Generic;
using KY.Core;
using KY.Core.Dependency;
using KY.Generator.Configurations;
using KY.Generator.Csharp.Languages;
using KY.Generator.EntityFramework.Configurations;
using KY.Generator.Output;
using KY.Generator.Templates;
using KY.Generator.Transfer;
using KY.Generator.Transfer.Writers;

namespace KY.Generator.EntityFramework.Writers
{
    public class EntityFrameworkWriter : ITransferWriter
    {
        private readonly IDependencyResolver resolver;

        public EntityFrameworkWriter(IDependencyResolver resolver)
        {
            this.resolver = resolver;
        }

        public virtual void Write(ConfigurationBase configurationBase, List<ITransferObject> transferObjects, IOutput output)
        {
            EntityFrameworkWriteConfiguration configuration = (EntityFrameworkWriteConfiguration)configurationBase;
            if (!configuration.Language.IsCsharp())
            {
                throw new InvalidOperationException("EntityFramework support only Csharp");
            }
            configuration.Namespace.AssertIsNotNull(nameof(configuration.Namespace), "ef and ef-core writer requires a namespace");

            List<FileTemplate> files = new List<FileTemplate>();
            if (configuration.Repositories.Count > 0)
            {
                this.resolver.Create<EntityFrameworkRepositoryWriter>().Write(configuration, transferObjects, files);
            }
            this.resolver.Create<EntityFrameworkDataContextWriter>().Write(configuration, transferObjects, files);
            files.ForEach(file => configuration.Language.Write(file, output));
        }
    }
}