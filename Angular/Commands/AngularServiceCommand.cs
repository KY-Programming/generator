using System.Collections.Generic;
using System.Linq;
using KY.Core.Dependency;
using KY.Generator.Angular.Configurations;
using KY.Generator.Angular.Writers;
using KY.Generator.Command;
using KY.Generator.Output;
using KY.Generator.Templates;
using KY.Generator.Transfer;

namespace KY.Generator.Angular.Commands
{
    internal class AngularServiceCommand : GeneratorCommand<AngularServiceCommandParameters>
    {
        private readonly IDependencyResolver resolver;
        public override string[] Names { get; } = { "angular-service" };

        public AngularServiceCommand(IDependencyResolver resolver)
        {
            this.resolver = resolver;
        }

        public override IGeneratorCommandResult Run(IOutput output)
        {
            AngularWriteConfiguration writeConfiguration = new AngularWriteConfiguration();
            writeConfiguration.AddHeader = !this.Parameters.SkipHeader;
            writeConfiguration.FormatNames = this.Parameters.FormatNames;
            writeConfiguration.OutputId = this.TransferObjects.OfType<OutputIdTransferObject>().FirstOrDefault()?.Value;
            writeConfiguration.Service = new AngularWriteServiceConfiguration();
            writeConfiguration.Service.Name = this.Parameters.Name;
            writeConfiguration.Service.RelativePath = this.Parameters.RelativePath;
            writeConfiguration.Service.EndlessTries = this.Parameters.EndlessTries;
            writeConfiguration.Service.Timeouts = this.Parameters.Timeouts;
            writeConfiguration.Model.RelativePath = this.Parameters.RelativeModelPath;

            output.DeleteAllRelatedFiles(writeConfiguration.OutputId, this.Parameters.RelativePath);

            List<FileTemplate> files = new List<FileTemplate>();
            this.resolver.Create<AngularServiceWriter>().Write(writeConfiguration, this.TransferObjects, files);
            IOutput localOutput = output;
            files.ForEach(file => writeConfiguration.Language.Write(file, localOutput));

            return this.Success();
        }
    }
}