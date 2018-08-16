using System.Collections.Generic;
using System.Linq;
using KY.Core;
using KY.Generator.Configuration;
using KY.Generator.Templates;

namespace KY.Generator.Command
{
    internal class CommandGenerator : IGenerator
    {
        private readonly List<ICommandGenerator> generators;
        public List<FileTemplate> Files { get; }

        public CommandGenerator(List<ICommandGenerator> generators)
        {
            this.generators = generators;
            this.Files = new List<FileTemplate>();
        }

        public void Generate(ConfigurationBase configurationBase)
        {
            this.Files.Clear();
            CommandConfiguration configuration = configurationBase as CommandConfiguration;
            if (configuration == null)
            {
                return;
            }
            this.generators.Where(x => x.Command.Equals(configuration.Command)).ForEach(x => x.Generate(configuration, this.Files));
        }
    }
}