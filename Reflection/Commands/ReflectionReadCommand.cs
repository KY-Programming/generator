using KY.Core.Dependency;
using KY.Generator.Command;
using KY.Generator.Models;
using KY.Generator.Output;
using KY.Generator.Reflection.Configurations;
using KY.Generator.Reflection.Readers;

namespace KY.Generator.Reflection.Commands
{
    public class ReflectionReadCommand : IGeneratorCommand, IUseGeneratorCommandEnvironment
    {
        private readonly IDependencyResolver resolver;
        public string[] Names { get; } = { "reflection-read" };
        public GeneratorEnvironment Environment { get; set; }

        public ReflectionReadCommand(IDependencyResolver resolver)
        {
            this.resolver = resolver;
        }

        public bool Generate(CommandConfiguration configuration, ref IOutput output)
        {
            ReflectionReadConfiguration readConfiguration = new ReflectionReadConfiguration();
            readConfiguration.CopyBaseFrom(configuration);
            readConfiguration.Assembly = configuration.Parameters.GetString(nameof(ReflectionReadConfiguration.Assembly));
            readConfiguration.Namespace = configuration.Parameters.GetString(nameof(ReflectionReadConfiguration.Namespace));
            readConfiguration.Name = configuration.Parameters.GetString(nameof(ReflectionReadConfiguration.Name));
            readConfiguration.SkipSelf = configuration.Parameters.GetBool(nameof(ReflectionReadConfiguration.SkipSelf));

            this.resolver.Create<ReflectionReader>().Read(readConfiguration, this.Environment.TransferObjects);
            return true;
        }
    }
}