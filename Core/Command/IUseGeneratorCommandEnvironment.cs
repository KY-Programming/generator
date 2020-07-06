using KY.Generator.Models;

namespace KY.Generator.Command
{
    public interface IUseGeneratorCommandEnvironment
    {
        GeneratorEnvironment Environment { get; set; }
    }
}