using System.Collections.Generic;
using CustomModule.Writers;
using KY.Core.Dependency;
using KY.Generator.Command;
using KY.Generator.Csharp.Languages;
using KY.Generator.Output;
using KY.Generator.Templates;

namespace CustomModule.Commands
{
    // 7. Create a Command
    //  a. Derive from GeneratorCommand
    //  b. Set one or more names (for the CLI)
    //  c. Implement Run method
    internal class WriteCommand : GeneratorCommand<WriteCommandParameters>
    {
        private readonly IDependencyResolver resolver;
        public override string[] Names { get; } = { "hello-world" };

        public WriteCommand(IDependencyResolver resolver)
        {
            this.resolver = resolver;
        }

        public override IGeneratorCommandResult Run(IOutput output)
        {
            // Create a instance of your Writer and run the action
            List<FileTemplate> files = new();
            this.resolver.Create<Writer>().Write(this.Parameters, files);
            files.ForEach(file => CsharpLanguage.Instance.Write(file, output));
            return this.Success();
        }
    }
}