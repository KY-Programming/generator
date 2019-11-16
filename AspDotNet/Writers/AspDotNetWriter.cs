using System.Collections.Generic;
using KY.Core.Dependency;
using KY.Generator.AspDotNet.Configurations;
using KY.Generator.Configuration;
using KY.Generator.Configurations;
using KY.Generator.Output;
using KY.Generator.Templates;
using KY.Generator.Transfer;
using KY.Generator.Transfer.Writers;

namespace KY.Generator.AspDotNet.Writers
{
    public class AspDotNetWriter : ITransferWriter
    {
        private readonly IDependencyResolver resolver;

        public AspDotNetWriter(IDependencyResolver resolver)
        {
            this.resolver = resolver;
        }

        public virtual void Write(ConfigurationBase configurationBase, List<ITransferObject> transferObjects, IOutput output)
        {
            AspDotNetWriteConfiguration configuration = (AspDotNetWriteConfiguration)configurationBase;
            List<FileTemplate> files = new List<FileTemplate>();
            if (configuration.GeneratorController != null)
            {
                this.resolver.Create<AspDotNetGeneratorControllerWriter>().Write(configuration, files);
            }
            if (configuration.Controllers.Count > 0)
            {
                this.resolver.Create<AspDotNetEntityControllerWriter>().Write(configuration, transferObjects, files);
            }
            files.ForEach(file => configuration.Language.Write(file, output));
        }
    }
}