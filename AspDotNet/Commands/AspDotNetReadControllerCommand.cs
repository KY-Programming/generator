using System.Collections.Generic;
using KY.Core.Dependency;
using KY.Generator.AspDotNet.Configurations;
using KY.Generator.AspDotNet.Readers;
using KY.Generator.Command;
using KY.Generator.Command.Extensions;
using KY.Generator.Models;
using KY.Generator.Output;
using KY.Generator.Transfer;

namespace KY.Generator.AspDotNet.Commands
{
    internal class AspDotNetReadControllerCommand : GeneratorCommand<AspDotNetReadControllerCommandParameters>
    {
        private readonly IDependencyResolver resolver;
        public override string[] Names { get; } = { "asp-read-controller" };

        public AspDotNetReadControllerCommand(IDependencyResolver resolver)
        {
            this.resolver = resolver;
        }

        public override IGeneratorCommandResult Run()
        {
            IOptions options = this.resolver.Get<Options>().Current;
            options.SetFromParameter(this.Parameters);

            AspDotNetReadConfiguration readConfiguration = new();
            readConfiguration.Controller = new AspDotNetReadControllerConfiguration();
            readConfiguration.Controller.Namespace = this.Parameters.Namespace;
            readConfiguration.Controller.Name = this.Parameters.Name;

            this.resolver.Create<AspDotNetControllerReader>().Read(readConfiguration);

            return this.Success();
        }
    }
}
