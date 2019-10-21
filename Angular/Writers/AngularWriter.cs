using System.Collections.Generic;
using KY.Core.Dependency;
using KY.Generator.Angular.Configurations;
using KY.Generator.Configuration;
using KY.Generator.Output;
using KY.Generator.Templates;
using KY.Generator.Transfer;
using KY.Generator.Transfer.Writers;

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
            AngularWriteConfiguration configuration = (AngularWriteConfiguration)configurationBase;
            List<FileTemplate> files = new List<FileTemplate>();
            if (configuration.Service != null)
            {
                this.resolver.Create<AngularServiceWriter>().Write(configuration, transferObjects, files);
            }
            if (configuration.WriteModels)
            {
                this.resolver.Create<AngularModelWriter>().Write(configuration, transferObjects, files);
            }
            files.ForEach(file => configuration.Language.Write(file, output));
        }
    }
}