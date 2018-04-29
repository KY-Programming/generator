namespace KY.Generator.Reflection.Configuration
{
    internal class ReflectionType
    {
        public string Assembly { get; set; }
        public string Name { get; set; }
        public string RelativePath { get; set; }
        public string Namespace { get; set; }
        public string Using { get; set; }
        public bool PropertiesToFields { get; set; }
        public bool FieldsToProperties { get; set; }
        public bool SkipNamespace { get; set; }
        public ReflectionConfiguration Configuration { get; set; }
    }
}