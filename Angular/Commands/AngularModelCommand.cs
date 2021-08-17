using KY.Core.Dependency;
using KY.Generator.Angular.Languages;
using KY.Generator.Angular.Writers;
using KY.Generator.Command;
using KY.Generator.Command.Extensions;
using KY.Generator.Languages.Extensions;
using KY.Generator.Output;
using KY.Generator.TypeScript;

namespace KY.Generator.Angular.Commands
{
    public class AngularModelCommand : GeneratorCommand<AngularModelCommandParameters>
    {
        private readonly IDependencyResolver resolver;

        public override string[] Names { get; } = { "angular-model" };

        public AngularModelCommand(IDependencyResolver resolver)
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

            output.DeleteAllRelatedFiles(options.OutputId, this.Parameters.RelativePath);

            this.resolver.Create<AngularModelWriter>().Write(this.TransferObjects, this.Parameters.RelativePath, output);

            return this.Success();
        }
    }
}
