using KY.Core.Dependency;
using KY.Generator.Command;
using KY.Generator.Command.Extensions;
using KY.Generator.Models;
using KY.Generator.Output;
using KY.Generator.Reflection.Configurations;
using KY.Generator.Reflection.Readers;

namespace KY.Generator.Reflection.Commands
{
    public class ReflectionReadCommand : IGeneratorCommand
    {
        private readonly IDependencyResolver resolver;
        private readonly GeneratorEnvironment environment;
        public string[] Names { get; } = { "reflection-read" };

        public ReflectionReadCommand(IDependencyResolver resolver, GeneratorEnvironment environment)
        {
            this.resolver = resolver;
            this.environment = environment;
        }

        public bool Generate(CommandConfiguration configuration, ref IOutput output)
        {
            ReflectionReadConfiguration readConfiguration = new ReflectionReadConfiguration();
            readConfiguration.ReadFromParameters(configuration.Parameters);
            readConfiguration.CopyBaseFrom(configuration);
            readConfiguration.Assembly = configuration.Parameters.GetString(nameof(ReflectionReadConfiguration.Assembly));
            readConfiguration.Namespace = configuration.Parameters.GetString(nameof(ReflectionReadConfiguration.Namespace));
            readConfiguration.Name = configuration.Parameters.GetString(nameof(ReflectionReadConfiguration.Name));
            readConfiguration.SkipSelf = configuration.Parameters.GetBool(nameof(ReflectionReadConfiguration.SkipSelf));

            this.resolver.Create<ReflectionReader>().Read(readConfiguration, this.environment.TransferObjects);
            return true;
        }
    }
}