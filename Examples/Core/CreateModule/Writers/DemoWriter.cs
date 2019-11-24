using System.Collections.Generic;
using CreateModule.Configurations;
using KY.Core.Dependency;
using KY.Generator.Configurations;
using KY.Generator.Output;
using KY.Generator.Templates;
using KY.Generator.Transfer;
using KY.Generator.Transfer.Writers;

namespace CreateModule.Writers
{
    internal class DemoWriter : ITransferWriter
    {
        private readonly IDependencyResolver resolver;

        public DemoWriter(IDependencyResolver resolver)
        {
            this.resolver = resolver;
        }

        public void Write(ConfigurationBase configurationBase, List<ITransferObject> transferObjects, IOutput output)
        {
            DemoWriteConfiguration configuration = (DemoWriteConfiguration)configurationBase;
            List<FileTemplate> files = new List<FileTemplate>();
            
            // You can add here multiple writers
            this.resolver.Create<DemoModelWriter>().Write(configuration, transferObjects, files);
            
            files.ForEach(file => configuration.Language.Write(file, output));
        }
    }
}