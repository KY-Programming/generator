using System.Collections.Generic;
using KY.Core.Dependency;
using KY.Generator.AspDotNet.Configurations;
using KY.Generator.Configurations;
using KY.Generator.Transfer;
using KY.Generator.Transfer.Readers;

namespace KY.Generator.AspDotNet.Readers
{
    public class AspDotNetReader : ITransferReader
    {
        private readonly IDependencyResolver resolver;

        public AspDotNetReader(IDependencyResolver resolver)
        {
            this.resolver = resolver;
        }

        public virtual void Read(AspDotNetReadConfiguration configuration, List<ITransferObject> transferObjects)
        {
            if (configuration.Controller != null)
            {
                this.resolver.Create<AspDotNetControllerReader>().Read(configuration, transferObjects);
            }
            if (configuration.Hub != null)
            {
                this.resolver.Create<AspDotNetHubReader>().Read(configuration, transferObjects);
            }
        }
    }
}
