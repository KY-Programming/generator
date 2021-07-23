using KY.Generator.Transfer;

namespace KY.Generator.TypeScript.Transfer
{
    public class TsConfig : ITransferObject
    {
        public CompilerOptions CompilerOptions { get; set; }
    }

    public class CompilerOptions
    {
        public bool Strict { get; set; }
    }
}
