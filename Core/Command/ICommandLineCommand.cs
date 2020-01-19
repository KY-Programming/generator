using KY.Generator.Configuration;

namespace KY.Generator.Command
{
    /// <summary>
    /// Command executed from the cli e.g. "KY-Programming.exe demo"
    /// </summary>
    public interface ICommandLineCommand : ICommand
    {
        bool Execute(IConfiguration configurationBase);
    }
}