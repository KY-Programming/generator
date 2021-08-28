using KY.Core.Dependency;
using KY.Generator.Angular.Languages;
using KY.Generator.Angular.Writers;
using KY.Generator.Command;
using KY.Generator.Command.Extensions;
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

        public override void Prepare()
        {
            IOptions options = this.resolver.Get<Options>().Current;
            options.SetFromParameter(this.Parameters);
            options.SetStrict(this.Parameters.RelativePath, this.resolver);
            options.Language = this.resolver.Get<AngularTypeScriptLanguage>();
            options.Formatting.AllowedSpecialCharacters = "$";
            options.SkipNamespace = true;
            options.PropertiesToFields = true;

            this.resolver.Create<AngularModelWriter>().FormatNames();
        }

        public override IGeneratorCommandResult Run()
        {
            this.resolver.Get<IOutput>().DeleteAllRelatedFiles(this.Parameters.RelativePath);
            this.resolver.Create<AngularModelWriter>().Write(this.Parameters.RelativePath);
            return this.Success();
        }
    }
}
