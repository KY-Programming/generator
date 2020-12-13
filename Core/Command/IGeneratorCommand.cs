using System.Collections.Generic;
using KY.Generator.Output;
using KY.Generator.Transfer;

namespace KY.Generator.Command
{
    public interface IGeneratorCommand
    {
        string[] Names { get; }
        GeneratorCommandParameters Parameters { get; }
        List<ITransferObject> TransferObjects { get; set; }

        bool Parse(IEnumerable<RawCommandParameter> parameters);
        bool Parse(params RawCommandParameter[] parameters);
        IGeneratorCommandResult Run(IOutput output);
    }
}