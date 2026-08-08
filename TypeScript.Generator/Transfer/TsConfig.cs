using KY.Generator.Transfer;

namespace KY.Generator.TypeScript.Transfer
{
    public class TsConfig : ITransferObject
    {
        public string Path { get; set; }
        public CompilerOptions CompilerOptions { get; set; } = new();
    }

    public class CompilerOptions
    {
        /// <summary>
        /// Null when the tsconfig.json does not mention "strict" at all. That is not the same as false: since
        /// TypeScript 6 the compiler defaults it to true, so an absent entry must not switch the strict mode off.
        /// </summary>
        public bool? Strict { get; set; }
    }
}
