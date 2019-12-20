using KY.Generator.Output;

namespace KY.Generator.Command
{
    public interface IGeneratorCommand
    {
        string[] Names { get; }
        bool Generate(CommandConfiguration configuration, ref IOutput output);
    }
}