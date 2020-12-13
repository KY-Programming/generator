using System;
using System.Linq;
using KY.Core;
using KY.Generator.Command;
using KY.Generator.Output;

namespace KY.Generator.Commands
{
    internal class VersionCommand : GeneratorCommand<VersionCommandParameters>
    {
        public override string[] Names { get; } = { "version", "v" };

        public override IGeneratorCommandResult Run(IOutput output)
        {
            Logger.Trace("Execute version command...");
            Logger.Trace("Loaded assemblies:");

            AppDomain.CurrentDomain.GetAssemblies()
                     .Select(x => x.GetName())
                     .OrderBy(x => x.Name)
                     .ForEach(x => Logger.Trace($"{x.Name} {x.Version} {(this.Parameters.ShowDetailed ? x.CodeBase.TrimStart("file:///") : "")}"));
            return this.Success();
        }
    }
}