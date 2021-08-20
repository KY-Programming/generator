namespace KY.Generator.Command
{
    public class GeneratorCommandParameters
    {
        [GeneratorGlobalParameter]
        public string Output { get; set; }

        [GeneratorGlobalParameter("beforeBuild")]
        public bool IsBeforeBuild { get; set; }

        [GeneratorGlobalParameter("msbuild")]
        public bool IsMsBuild { get; set; }

        [GeneratorGlobalParameter("onlyAsync")]
        public bool IsOnlyAsync { get; set; }

        [GeneratorGlobalParameter("async")]
        public bool IsAsync { get; set; }

        public bool? IsAsyncAssembly { get; set; }

        [GeneratorGlobalParameter]
        public bool SkipHeader { get; set; }

        [GeneratorGlobalParameter]
        public bool VerifySsl { get; set; }

        [GeneratorGlobalParameter]
        public bool SkipAsyncCheck { get; set; }

        public string Assembly { get; set; }
        public string RelativePath { get; set; }
        public bool SkipNamespace { get; set; } = true;
        public bool PropertiesToFields { get; set; } = true;
        public bool FieldsToProperties { get; set; }
        public bool PreferInterfaces { get; set; }
        public bool WithOptionalProperties { get; set; }
        public bool FormatNames { get; set; } = true;
    }
}
