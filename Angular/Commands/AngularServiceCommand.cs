using System.Collections.Generic;
using KY.Core.Dependency;
using KY.Generator.Angular.Configurations;
using KY.Generator.Angular.Writers;
using KY.Generator.Command;
using KY.Generator.Command.Extensions;
using KY.Generator.Models;
using KY.Generator.Output;
using KY.Generator.Templates;

namespace KY.Generator.Angular.Commands
{
    internal class AngularServiceCommand : IGeneratorCommand
    {
        private readonly IDependencyResolver resolver;
        private readonly GeneratorEnvironment environment;
        public string[] Names { get; } = { "angular-service" };

        public AngularServiceCommand(IDependencyResolver resolver, GeneratorEnvironment environment)
        {
            this.resolver = resolver;
            this.environment = environment;
        }

        public bool Generate(CommandConfiguration configuration, ref IOutput output)
        {
            AngularWriteConfiguration writeConfiguration = new AngularWriteConfiguration(configuration);
            writeConfiguration.ReadFromParameters(configuration.Parameters);
            writeConfiguration.FormatNames = configuration.Parameters.GetBool(nameof(AngularWriteConfiguration.FormatNames), true);
            writeConfiguration.Service = new AngularWriteServiceConfiguration();
            writeConfiguration.Service.Name = configuration.Parameters.GetString(nameof(AngularWriteServiceConfiguration.Name));
            writeConfiguration.Service.RelativePath = configuration.Parameters.GetString(nameof(AngularWriteServiceConfiguration.RelativePath));
            writeConfiguration.Service.EndlessTries = configuration.Parameters.GetBool(nameof(AngularWriteServiceConfiguration.EndlessTries));
            writeConfiguration.Service.Timeouts = configuration.Parameters.GetInts(nameof(AngularWriteServiceConfiguration.Timeouts));
            writeConfiguration.Model.RelativePath = configuration.Parameters.GetString("RelativeModelPath");
            
            List<FileTemplate> files = new List<FileTemplate>();
            this.resolver.Create<AngularServiceWriter>().Write(writeConfiguration, this.environment.TransferObjects, files);
            IOutput localOutput = output;
            files.ForEach(file => writeConfiguration.Language.Write(file, localOutput));

            return true;
        }
    }
}