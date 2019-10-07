using System.Collections.Generic;
using KY.Core.Dependency;
using KY.Generator.Angular.Configurations;
using KY.Generator.Configuration;
using KY.Generator.Output;
using KY.Generator.Transfer;

namespace KY.Generator.Angular.Writers
{
    internal class AngularWriter : ITransferWriter
    {
        private readonly IDependencyResolver resolver;

        public AngularWriter(IDependencyResolver resolver)
        {
            this.resolver = resolver;
        }

        public void Write(ConfigurationBase configurationBase, List<ITransferObject> transferObjects, IOutput output)
        {
            AngularConfiguration configuration = (AngularConfiguration)configurationBase;
            if (configuration.Service != null)
            {
                this.resolver.Create<AngularServiceWriter>().Write(configuration, transferObjects, output);
            }
        }
    }
}