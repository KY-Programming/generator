using System.Collections.Generic;
using System.Linq;
using KY.Core;
using KY.Core.Dependency;
using KY.Generator.Angular.Configurations;
using KY.Generator.Angular.Languages;
using KY.Generator.Angular.Writers;
using KY.Generator.Command;
using KY.Generator.Command.Extensions;
using KY.Generator.Languages.Extensions;
using KY.Generator.Mappings;
using KY.Generator.Output;
using KY.Generator.Templates;
using KY.Generator.Transfer;
using KY.Generator.TypeScript;
using KY.Generator.TypeScript.Transfer;

namespace KY.Generator.Angular.Commands
{
    public class AngularServiceCommand : GeneratorCommand<AngularServiceCommandParameters>
    {
        private readonly IDependencyResolver resolver;
        public override string[] Names { get; } = { "angular-service" };

        public AngularServiceCommand(IDependencyResolver resolver)
        {
            this.resolver = resolver;
        }

        public override IGeneratorCommandResult Run(IOutput output)
        {
            IOptions options = this.resolver.Get<Options>().Current;
            options.SetFromParameter(this.Parameters);
            options.SetOutputId(this.TransferObjects);
            options.SetStrict(this.Parameters.RelativePath, output, this.resolver, this.TransferObjects);
            options.Language = new AngularTypeScriptLanguage();
            options.Formatting.FromLanguage(options.Language);
            options.Formatting.AllowedSpecialCharacters = "$";
            options.SkipNamespace = true;
            options.PropertiesToFields = true;

            AngularWriteConfiguration writeConfiguration = new();
            writeConfiguration.Service = new AngularWriteServiceConfiguration();
            writeConfiguration.Service.Name = this.Parameters.Name;
            writeConfiguration.Service.RelativePath = this.Parameters.RelativePath;
            writeConfiguration.Service.EndlessTries = this.Parameters.EndlessTries;
            writeConfiguration.Service.Timeouts = this.Parameters.Timeouts;
            writeConfiguration.Model.RelativePath = this.Parameters.RelativeModelPath;

            output.DeleteAllRelatedFiles(options.OutputId, this.Parameters.RelativePath);

            this.resolver.Create<AngularServiceWriter>().Write(this.TransferObjects, writeConfiguration, output);

            return this.Success();
        }
    }
}
