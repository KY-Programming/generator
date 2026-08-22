using KY.Core;
using KY.Generator.Command;

namespace KY.Generator.Commands;

internal class GetLicenseCommand(SettingsService settingsService) : GeneratorCommand<GetLicenseCommandParameters>
{
    public override Task<IGeneratorCommandResult> Run()
    {
        Logger.Trace("Execute license command...");
        Logger.Trace("Current license id: " + settingsService.License);
        return this.SuccessAsync();
    }
}
