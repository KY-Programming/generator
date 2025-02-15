using KY.Core;
using KY.Generator.Command;
using KY.Generator.Settings;

namespace KY.Generator.Commands;

internal class GetLicenseCommand : GeneratorCommand<GetLicenseCommandParameters>
{
    private readonly GlobalSettingsService globalSettingsService;
    public static string[] Names { get; } = [ToCommand(nameof(GetLicenseCommand)), "get-license", "l"];

    public GetLicenseCommand(GlobalSettingsService globalSettingsService)
    {
        this.globalSettingsService = globalSettingsService;
    }

    public override IGeneratorCommandResult Run()
    {
        Logger.Trace("Execute license command...");
        Logger.Trace("Current license id: " + this.globalSettingsService.Read().License);
        return this.Success();
    }
}
