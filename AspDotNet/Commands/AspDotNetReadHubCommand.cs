using KY.Core.Dependency;
using KY.Generator.AspDotNet.Configurations;
using KY.Generator.AspDotNet.Readers;
using KY.Generator.Command;
using KY.Generator.Output;

namespace KY.Generator.AspDotNet.Commands
{
    internal class AspDotNetReadHubCommand : GeneratorCommand<AspDotNetReadHubCommandParameters>
    {
        private readonly IDependencyResolver resolver;
        public override string[] Names { get; } = { "asp-read-hub" };

        public AspDotNetReadHubCommand(IDependencyResolver resolver)
        {
            this.resolver = resolver;
        }

        public override IGeneratorCommandResult Run(IOutput output)
        {
            AspDotNetReadConfiguration readConfiguration = new AspDotNetReadConfiguration();
            readConfiguration.AddHeader = !this.Parameters.SkipHeader;
            readConfiguration.Hub = new AspDotNetReadHubConfiguration();
            readConfiguration.Hub.Namespace = this.Parameters.Namespace;
            readConfiguration.Hub.Name = this.Parameters.Name;

            this.resolver.Create<AspDotNetHubReader>().Read(readConfiguration, this.TransferObjects);

            return this.Success();
        }
    }
}