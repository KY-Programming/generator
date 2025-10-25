using KY.Core;
using KY.Generator.Command;

namespace KY.Generator.Commands;

internal class VersionCommand : GeneratorCommand<VersionCommandParameters>
{
    public static string[] Names { get; } = [..ToCommand(nameof(VersionCommand)), "version", "v"];

    public override IGeneratorCommandResult Run()
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
