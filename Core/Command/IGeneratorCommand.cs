using KY.Generator.Configurations;

namespace KY.Generator.Command
{
    public interface IGeneratorCommand
    {
        bool Execute(IConfiguration configurationBase);
    }
}