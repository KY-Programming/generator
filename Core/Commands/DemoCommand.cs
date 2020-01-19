using KY.Core;
using KY.Generator.Command;
using KY.Generator.Configuration;
using KY.Generator.Configurations;

namespace KY.Generator.Commands
{
    public class DemoCommand : ICommandLineCommand
    {
        public bool Execute(IConfiguration configurationBase)
        {
            DemoConfiguration configuration = (DemoConfiguration)configurationBase;
            Logger.Trace(configuration.Message);

            Logger.Trace("See full documentation on https://github.com/KY-Programming/generator/wiki");
            return true;
        }
    }
}