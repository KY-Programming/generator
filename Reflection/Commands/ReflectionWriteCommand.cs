using System.Linq;
using KY.Core.Dependency;
using KY.Generator.Command;
using KY.Generator.Configurations;
using KY.Generator.Csharp.Languages;
using KY.Generator.Output;
using KY.Generator.Reflection.Configurations;
using KY.Generator.Reflection.Writers;
using KY.Generator.Transfer;

namespace KY.Generator.Reflection.Commands
{
    internal class ReflectionWriteCommand : GeneratorCommand<ReflectionWriteCommandParameters>
    {
        private readonly IDependencyResolver resolver;

        public override string[] Names { get; } = { "reflection-write" };

        public ReflectionWriteCommand(IDependencyResolver resolver)
        {
            this.resolver = resolver;
        }

        public override IGeneratorCommandResult Run(IOutput output)
        {
            ReflectionWriteConfiguration configuration = new ReflectionWriteConfiguration();
            configuration.FormatNames = this.Parameters.FormatNames;
            configuration.FieldsToProperties = this.Parameters.FieldsToProperties;
            configuration.PropertiesToFields = this.Parameters.PropertiesToFields;
            configuration.Namespace = this.Parameters.Namespace;
            configuration.Name = this.Parameters.Name;
            configuration.RelativePath = this.Parameters.RelativePath;
            configuration.OutputId = this.TransferObjects.OfType<OutputIdTransferObject>().FirstOrDefault()?.Value;
            configuration.SkipNamespace = this.Parameters.SkipNamespace;
            configuration.Language = CsharpLanguage.Instance;

            this.resolver.Create<ReflectionWriter>().Write(configuration, this.TransferObjects, output);

            return this.Success();
        }
    }
}
