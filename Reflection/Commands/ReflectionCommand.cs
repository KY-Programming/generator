using System;
using System.Collections.Generic;
using KY.Core.Dependency;
using KY.Generator.Command;
using KY.Generator.Command.Extensions;
using KY.Generator.Csharp.Languages;
using KY.Generator.Extensions;
using KY.Generator.Output;
using KY.Generator.Reflection.Configurations;
using KY.Generator.Reflection.Readers;
using KY.Generator.Reflection.Writers;
using KY.Generator.Transfer;
using KY.Generator.TypeScript.Languages;

namespace KY.Generator.Reflection.Commands
{
    internal class ReflectionCommand : GeneratorCommand<ReflectionCommandParameters>
    {
        private readonly IDependencyResolver resolver;
        public override string[] Names { get; } = { "reflection" };

        public ReflectionCommand(IDependencyResolver resolver)
        {
            this.resolver = resolver;
        }

        public override IGeneratorCommandResult Run()
        {
            IDependencyResolver attributeResolver = this.resolver.CloneForCommands();
            Options options = attributeResolver.Get<Options>();
            IOptions attributeOptions = options.Current;
            attributeOptions.SetFromParameter(this.Parameters);
            attributeOptions.Language = this.Parameters.Language?.Name?.Equals(nameof(OutputLanguage.Csharp), StringComparison.CurrentCultureIgnoreCase) ?? false
                                            ? this.resolver.Get<CsharpLanguage>()
                                            : this.resolver.Get<TypeScriptLanguage>();

            ReflectionReadConfiguration readConfiguration = new();
            readConfiguration.Name = this.Parameters.Name;
            readConfiguration.Namespace = this.Parameters.Namespace;
            readConfiguration.Assembly = this.Parameters.Assembly;
            readConfiguration.SkipSelf = this.Parameters.SkipSelf;

            attributeResolver.Create<ReflectionReader>().Read(readConfiguration, attributeOptions);
            attributeResolver.Get<IOutput>().DeleteAllRelatedFiles(this.Parameters.RelativePath);
            ReflectionWriter writer = attributeResolver.Create<ReflectionWriter>();
            writer.FormatNames();
            writer.Write(this.Parameters.RelativePath);

            this.resolver.Get<Models.Environment>().TransferObjects.AddIfNotExists(attributeResolver.Get<List<ITransferObject>>());

            return this.Success();
        }
    }
}
