using System;
using System.Collections.Generic;
using KY.Core;
using KY.Core.Dependency;
using KY.Generator.Command;
using KY.Generator.Configuration;
using KY.Generator.Csharp.Languages;
using KY.Generator.EntityFramework.Configurations;
using KY.Generator.EntityFramework.Writers;
using KY.Generator.Extensions;
using KY.Generator.Templates;
using KY.Generator.Transfer;

namespace KY.Generator.EntityFramework.Commands
{
    public class WriteEntityFrameworkCommand : IConfigurationCommand
    {
        private readonly IDependencyResolver resolver;

        public WriteEntityFrameworkCommand(IDependencyResolver resolver)
        {
            this.resolver = resolver;
        }

        public bool Execute(IConfiguration configurationBase, List<ITransferObject> transferObjects)
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
            files.Write(configuration);
            return true;
        }
    }
}