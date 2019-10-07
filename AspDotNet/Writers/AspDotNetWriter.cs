using System.Collections.Generic;
using KY.Core.Dependency;
using KY.Generator.AspDotNet.Configurations;
using KY.Generator.Configuration;
using KY.Generator.Output;
using KY.Generator.Transfer;

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
            AspDotNetWriteConfiguration configuration = (AspDotNetWriteConfiguration)configurationBase;
            if (configuration.Controller != null)
            {
                this.resolver.Create<AspDotNetGeneratorControllerWriter>().Write(configuration, output);
            }
        }
    }
}