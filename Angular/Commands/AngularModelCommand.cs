using System.Collections.Generic;
using KY.Core.Dependency;
using KY.Generator.Angular.Configurations;
using KY.Generator.Angular.Writers;
using KY.Generator.Command;
using KY.Generator.Models;
using KY.Generator.Output;
using KY.Generator.Templates;

namespace KY.Generator.Angular.Commands
{
    internal class AngularModelCommand : IGeneratorCommand
    {
        private readonly IDependencyResolver resolver;
        private readonly GeneratorEnvironment environment;
        public string[] Names { get; } = { "angular-model" };

        public AngularModelCommand(IDependencyResolver resolver, GeneratorEnvironment environment)
        {
            this.resolver = resolver;
            this.environment = environment;
        }

        public bool Generate(CommandConfiguration configuration, ref IOutput output)
        {
            AngularWriteConfiguration writeConfiguration = new AngularWriteConfiguration(configuration);
            writeConfiguration.FormatNames = configuration.Parameters.GetBool(nameof(AngularWriteConfiguration.FormatNames), true);
            writeConfiguration.Model = new AngularWriteModelConfiguration();
            writeConfiguration.Model.Namespace = configuration.Parameters.GetString(nameof(AngularWriteModelConfiguration.Namespace));
            writeConfiguration.Model.RelativePath = configuration.Parameters.GetString(nameof(AngularWriteModelConfiguration.RelativePath));
            writeConfiguration.Model.SkipNamespace = configuration.Parameters.GetBool(nameof(AngularWriteModelConfiguration.SkipNamespace), true);
            writeConfiguration.Model.PropertiesToFields = configuration.Parameters.GetBool(nameof(AngularWriteModelConfiguration.PropertiesToFields), true);
            writeConfiguration.Model.FieldsToProperties = configuration.Parameters.GetBool(nameof(AngularWriteModelConfiguration.FieldsToProperties));
            writeConfiguration.Model.FormatNames = configuration.Parameters.GetBool(nameof(AngularWriteModelConfiguration.FormatNames), true);

            List<FileTemplate> files = new List<FileTemplate>();
            this.resolver.Create<AngularModelWriter>().Write(writeConfiguration, this.environment.TransferObjects, files);
            IOutput localOutput = output;
            files.ForEach(file => writeConfiguration.Language.Write(file, localOutput));

            return true;
        }
    }
}