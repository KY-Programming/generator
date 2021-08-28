using KY.Core.Dependency;
using KY.Generator.Command;
using KY.Generator.Command.Extensions;
using KY.Generator.Csharp.Languages;
using KY.Generator.Output;
using KY.Generator.Reflection.Writers;

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

        public override IGeneratorCommandResult Run()
        {
            IOptions options = this.resolver.Get<Options>().Current;
            options.SetFromParameter(this.Parameters);
            options.Language = this.resolver.Get<CsharpLanguage>();
            ReflectionWriter writer = this.resolver.Create<ReflectionWriter>();
            writer.FormatNames();
            this.resolver.Get<IOutput>().DeleteAllRelatedFiles(this.Parameters.RelativePath);
            writer.Write(this.Parameters.RelativePath);

            return this.Success();
        }
    }
}
