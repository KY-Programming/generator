using System.Collections.Generic;
using KY.Generator.Templates;

namespace KY.Generator.Command
{
    public interface ICommandGenerator
    {
        string Command { get; }
        void Generate(CommandConfiguration configuration, IList<FileTemplate> files);
    }
}