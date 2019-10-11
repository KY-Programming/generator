using System;
using System.Linq;
using KY.Core;
using KY.Generator.Command;
using KY.Generator.Output;

namespace KY.Generator.Commands
{
    internal class VersionCommand : IGeneratorCommand
    {
        public string[] Names { get; } = { "version", "-version", "--version", "v", "-v", "--v" };

        public bool Generate(CommandConfiguration configuration, IOutput output)
        {
            bool detailed = configuration.Parameters.GetBool("d");
            Logger.Trace("Execute version command...");
            Logger.Trace("Loaded assemblies:");

            AppDomain.CurrentDomain.GetAssemblies()
                     .Select(x => x.GetName())
                     .OrderBy(x => x.Name)
                     .ForEach(x => Logger.Trace($"{x.Name} {x.Version} {(detailed ? x.CodeBase.TrimStart("file:///") : "")}"));
            return true;
        }
    }
}