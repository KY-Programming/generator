using System.Collections.Generic;
using CreateModule.Configurations;
using KY.Core.Dependency;
using KY.Generator.Configurations;
using KY.Generator.Transfer;
using KY.Generator.Transfer.Readers;

namespace CreateModule.Readers
{
    public class DemoReader : ITransferReader
    {
        private readonly IDependencyResolver resolver;

        public DemoReader(IDependencyResolver resolver)
        {
            this.resolver = resolver;
        }

        public void Read(ConfigurationBase configurationBase, List<ITransferObject> transferObjects)
        {
            DemoReadConfiguration configuration = (DemoReadConfiguration)configurationBase;
            if (configuration.HelloWorld != null)
            {
                this.resolver.Create<HelloWorldReader>().Read(configuration, transferObjects);
            }
        }
    }
}