using KY.Core;
using KY.Generator.Command;
using KY.Generator.Licensing;
using KY.Generator.Settings;

namespace KY.Generator.Commands;

internal class GetLicenseCommand : GeneratorCommand<GetLicenseCommandParameters>
{
    private readonly GlobalSettingsService globalSettingsService;
    public override string[] Names { get; } = { "get-license", "l" };

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
