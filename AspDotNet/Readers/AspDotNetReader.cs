using System.Collections.Generic;
using KY.Core;
using KY.Core.Dependency;
using KY.Generator.AspDotNet.Configurations;
using KY.Generator.Configuration;
using KY.Generator.Transfer;

namespace KY.Generator.AspDotNet.Readers
{
    internal class AspDotNetReader : ITransferReader
    {
        private readonly IDependencyResolver resolver;

        public AspDotNetReader(IDependencyResolver resolver)
        {
            this.resolver = resolver;
        }

        public void Read(ConfigurationBase configurationBase, List<ITransferObject> transferObjects)
        {
            AspDotNetReadConfiguration configuration = (AspDotNetReadConfiguration)configurationBase;
            if (configuration.Controller != null)
            {
                this.resolver.Create<AspDotNetControllerReader>().Read(configuration).ForEach(transferObjects.Add);
            }
        }
    }
}