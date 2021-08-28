using System.Collections.Generic;
using KY.Core.Dependency;
using KY.Generator.AspDotNet.Configurations;
using KY.Generator.Templates;
using KY.Generator.Transfer.Writers;

namespace KY.Generator.AspDotNet.Writers
{
    public class AspDotNetWriter : ITransferWriter
    {
        private readonly IDependencyResolver resolver;
        private readonly Options options;

        public AspDotNetWriter(IDependencyResolver resolver, Options options)
        {
            this.resolver = resolver;
            this.options = options;
        }

        public virtual void Write(AspDotNetWriteConfiguration configuration)
        {
            List<FileTemplate> files = new List<FileTemplate>();
            if (configuration.GeneratorController != null)
            {
                this.resolver.Create<AspDotNetGeneratorControllerWriter>().Write(configuration, files);
            }
            if (configuration.Controllers.Count > 0)
            {
                this.resolver.Create<AspDotNetEntityControllerWriter>().Write(configuration);
            }
        }
    }
}
