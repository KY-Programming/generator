using System;
using System.Collections.Generic;
using KY.Core;
using KY.Core.Dependency;
using KY.Generator.AspDotNet.Configurations;
using KY.Generator.Configuration;
using KY.Generator.Csharp.Languages;
using KY.Generator.Output;
using KY.Generator.Templates;
using KY.Generator.Transfer;
using KY.Generator.Transfer.Writers;

namespace KY.Generator.AspDotNet.Writers
{
    internal class AspDotNetWriter : ITransferWriter
    {
        private readonly IDependencyResolver resolver;

        public AspDotNetWriter(IDependencyResolver resolver)
        {
            this.resolver = resolver;
        }

        public void Write(ConfigurationBase configurationBase, List<ITransferObject> transferObjects, IOutput output)
        {
            List<FileTemplate> files = new List<FileTemplate>();
            AspDotNetWriteConfiguration configuration = (AspDotNetWriteConfiguration)configurationBase;
            if (!configuration.Language.IsCsharp())
            {
                throw new InvalidOperationException("ASP.NET support only Csharp");
            }
            configuration.Namespace.AssertIsNotNull(nameof(configuration.Namespace), "asp writer requires a namespace");
            if (configuration.GeneratorController != null)
            {
                this.resolver.Create<AspDotNetGeneratorControllerWriter>().Write(configuration, files);
            }
            if (configuration.Controllers.Count > 0)
            {
                AspDotNetEntityControllerWriter controllerWriter = this.resolver.Create<AspDotNetEntityControllerWriter>();
                controllerWriter.Write(configuration, transferObjects, files);
            }
            files.ForEach(file => configuration.Language.Write(file, output));
        }
    }
}