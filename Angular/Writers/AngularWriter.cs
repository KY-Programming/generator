using System.Collections.Generic;
using KY.Core.Dependency;
using KY.Generator.Angular.Configurations;
using KY.Generator.Configuration;
using KY.Generator.Configurations;
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

        public void Write(AngularWriteConfiguration configuration, List<ITransferObject> transferObjects, IOutput output)
        {
            if (configuration.Service != null)
            {
                this.resolver.Create<AngularServiceWriter>().Write(transferObjects, configuration, output);
            }
            if (configuration.WriteModels)
            {
                this.resolver.Create<AngularModelWriter>().Write(transferObjects, configuration.Model.RelativePath, output);
            }
        }
    }
}
