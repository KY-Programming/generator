using System.Collections.Generic;
using KY.Core.Dependency;
using KY.Generator.Angular.Configurations;
using KY.Generator.Angular.Writers;
using KY.Generator.Command;
using KY.Generator.Output;
using KY.Generator.Templates;

namespace KY.Generator.Angular.Commands
{
    internal class AngularModelCommand : GeneratorCommand<AngularModelCommandParameters>
    {
        private readonly IDependencyResolver resolver;

        public override string[] Names { get; } = { "angular-model" };

        public AngularModelCommand(IDependencyResolver resolver)
        {
            this.resolver = resolver;
        }

        public override IGeneratorCommandResult Run(IOutput output)
        {
            AngularWriteConfiguration writeConfiguration = new AngularWriteConfiguration();
            writeConfiguration.AddHeader = !this.Parameters.SkipHeader;
            writeConfiguration.FormatNames = this.Parameters.FormatNames;
            writeConfiguration.Model = new AngularWriteModelConfiguration();
            writeConfiguration.Model.CopyBaseFrom(writeConfiguration);
            writeConfiguration.Model.Namespace = this.Parameters.Namespace;
            writeConfiguration.Model.RelativePath = this.Parameters.RelativePath;
            writeConfiguration.Model.SkipNamespace = this.Parameters.SkipNamespace;
            writeConfiguration.Model.PropertiesToFields = this.Parameters.PropertiesToFields;
            writeConfiguration.Model.FieldsToProperties = this.Parameters.FieldsToProperties;
            writeConfiguration.Model.FormatNames = this.Parameters.FormatNames;

            List<FileTemplate> files = new List<FileTemplate>();
            this.resolver.Create<AngularModelWriter>().Write(writeConfiguration, this.TransferObjects, files);
            IOutput localOutput = output;
            files.ForEach(file => writeConfiguration.Language.Write(file, localOutput));

            return this.Success();
        }
    }
}