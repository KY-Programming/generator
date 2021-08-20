using System;
using System.Collections.Generic;
using KY.Core.Dependency;
using KY.Generator.Command;
using KY.Generator.Command.Extensions;
using KY.Generator.Csharp.Languages;
using KY.Generator.Languages.Extensions;
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

        public override IGeneratorCommandResult Run(IOutput output)
        {
            DependencyResolver attributeResolver = new(this.resolver);
            Options.Bind(attributeResolver);

            IOptions options = attributeResolver.Get<Options>().Current;
            options.SetFromParameter(this.Parameters);
            options.SetOutputId(this.TransferObjects);
            options.Language = this.Parameters.Language?.Name?.Equals(nameof(OutputLanguage.Csharp), StringComparison.CurrentCultureIgnoreCase) ?? false ? CsharpLanguage.Instance : TypeScriptLanguage.Instance;
            options.Formatting.FromLanguage(options.Language);

            ReflectionReadConfiguration readConfiguration = new();
            readConfiguration.Name = this.Parameters.Name;
            readConfiguration.Namespace = this.Parameters.Namespace;
            readConfiguration.Assembly = this.Parameters.Assembly;
            readConfiguration.SkipSelf = this.Parameters.SkipSelf;

            List<ITransferObject> transferObjects = new();
            attributeResolver.Create<ReflectionReader>().Read(readConfiguration, transferObjects, options);
            output.DeleteAllRelatedFiles(options.OutputId, this.Parameters.RelativePath);
            attributeResolver.Create<ReflectionWriter>().Write(transferObjects, this.Parameters.RelativePath, output);

            return this.Success();
        }
    }
}
