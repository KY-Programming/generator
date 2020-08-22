using KY.Core.Dependency;
using KY.Generator.AspDotNet.Configurations;
using KY.Generator.AspDotNet.Readers;
using KY.Generator.Command;
using KY.Generator.Models;
using KY.Generator.Output;

namespace KY.Generator.AspDotNet.Commands
{
    internal class AspDotNetReadHubCommand : IGeneratorCommand
    {
        private readonly IDependencyResolver resolver;
        private readonly GeneratorEnvironment environment;
        public string[] Names { get; } = { "asp-read-hub" };

        public AspDotNetReadHubCommand(IDependencyResolver resolver, GeneratorEnvironment environment)
        {
            this.resolver = resolver;
            this.environment = environment;
        }

        public bool Generate(CommandConfiguration configuration, ref IOutput output)
        {
            AspDotNetReadConfiguration readConfiguration = new AspDotNetReadConfiguration();
            readConfiguration.Hub = new AspDotNetReadHubConfiguration();
            readConfiguration.Hub.Namespace = configuration.Parameters.GetString(nameof(AspDotNetReadHubConfiguration.Namespace));
            readConfiguration.Hub.Name = configuration.Parameters.GetString(nameof(AspDotNetReadHubConfiguration.Name));

            this.resolver.Create<AspDotNetHubReader>().Read(readConfiguration, this.environment.TransferObjects);

            return true;
        }
    }
}