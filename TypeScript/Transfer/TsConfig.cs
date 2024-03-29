﻿using KY.Generator.Transfer;

namespace KY.Generator.TypeScript.Transfer
{
    public class TsConfig : ITransferObject
    {
        public string Path { get; set; }
        public CompilerOptions CompilerOptions { get; set; } = new();
    }

    public class CompilerOptions
    {
        public bool Strict { get; set; }
    }
}
