using System.Collections.Generic;
using KY.Core.Dependency;
using KY.Generator.Command;
using KY.Generator.Command.Extensions;
using KY.Generator.Csharp.Languages;
using KY.Generator.Models;
using KY.Generator.Output;
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

        public override IGeneratorCommandResult Run()
        {
            IOptions options = this.resolver.Get<Options>().Current;
            options.SetFromParameter(this.Parameters);
            options.Language = this.resolver.Get<CsharpLanguage>();

            this.resolver.Create<ReflectionWriter>().Write(this.Parameters.RelativePath);

            return this.Success();
        }
    }
}
