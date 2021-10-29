using System.Collections.Generic;

namespace KY.Generator.Command
{
    public interface IGeneratorCommand
    {
        string[] Names { get; }
        GeneratorCommandParameters Parameters { get; }

        bool Parse(IEnumerable<RawCommandParameter> parameters);
        bool Parse(params RawCommandParameter[] parameters);
        void Prepare();
        void FollowUp();
        IGeneratorCommandResult Run();
    }
}
