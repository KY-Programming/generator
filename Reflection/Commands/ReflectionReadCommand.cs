using KY.Core.Dependency;
using KY.Generator.Command;
using KY.Generator.Output;
using KY.Generator.Reflection.Configurations;
using KY.Generator.Reflection.Readers;

namespace KY.Generator.Reflection.Commands
{
    public class ReflectionReadCommand : GeneratorCommand<ReflectionReadCommandParameters>
    {
        private readonly IDependencyResolver resolver;
        public override string[] Names { get; } = { "reflection-read" };

        public ReflectionReadCommand(IDependencyResolver resolver)
        {
            this.resolver = resolver;
        }

        public override IGeneratorCommandResult Run()
        {
            ReflectionReadConfiguration readConfiguration = new();
            readConfiguration.Assembly = this.Parameters.Assembly;
            readConfiguration.Namespace = this.Parameters.Namespace;
            readConfiguration.Name = this.Parameters.Name;
            readConfiguration.OnlySubTypes = this.Parameters.OnlySubTypes;

            this.resolver.Create<ReflectionReader>().Read(readConfiguration);
            return this.Success();
        }
    }
}
