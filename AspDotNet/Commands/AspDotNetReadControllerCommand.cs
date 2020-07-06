using KY.Core.Dependency;
using KY.Generator.AspDotNet.Configurations;
using KY.Generator.AspDotNet.Readers;
using KY.Generator.Command;
using KY.Generator.Models;
using KY.Generator.Output;

namespace KY.Generator.AspDotNet.Commands
{
    internal class AspDotNetReadControllerCommand : IGeneratorCommand, IUseGeneratorCommandEnvironment
    {
        private readonly IDependencyResolver resolver;
        public string[] Names { get; } = { "asp-read-controller" };
        public GeneratorEnvironment Environment { get; set; }

        public AspDotNetReadControllerCommand(IDependencyResolver resolver)
        {
            this.resolver = resolver;
        }

        public bool Generate(CommandConfiguration configuration, ref IOutput output)
        {
            AspDotNetReadConfiguration readConfiguration = new AspDotNetReadConfiguration();
            readConfiguration.Controller = new AspDotNetReadControllerConfiguration();
            readConfiguration.Controller.Namespace = configuration.Parameters.GetString(nameof(AspDotNetReadControllerConfiguration.Namespace));
            readConfiguration.Controller.Name = configuration.Parameters.GetString(nameof(AspDotNetReadControllerConfiguration.Name));

            this.resolver.Create<AspDotNetControllerReader>().Read(readConfiguration, this.Environment.TransferObjects);

            return true;
        }
    }
}