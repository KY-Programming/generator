using KY.Core.Dependency;
using KY.Generator.Angular.Configurations;
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

        public void Write(AngularWriteConfiguration configuration)
        {
            if (configuration.Service != null)
            {
                this.resolver.Create<AngularServiceWriter>().Write(configuration);
            }
            if (configuration.WriteModels)
            {
                this.resolver.Create<AngularModelWriter>().Write(configuration.Model.RelativePath);
            }
        }
    }
}
