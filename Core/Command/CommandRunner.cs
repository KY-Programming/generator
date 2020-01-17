using KY.Generator.Configurations;
using KY.Generator.Output;

namespace KY.Generator.Command
{
    public class CommandRunner
    {
        private readonly CommandRegister commands;

        public CommandRunner(CommandRegister commands)
        {
            this.commands = commands;
        }

        public bool Run(IConfiguration configuration, IOutput output)
        {
            if (configuration == null)
            {
                return false;
            }
            IGeneratorCommand command = this.commands.CreateCommand(configuration);
            bool success = command.Execute(configuration);
            if (success)
            {
                output.Execute();
            }
            return success;
        }
    }
}