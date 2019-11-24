using KY.Generator.Configurations;

namespace CreateModule.Configurations
{
    internal class DemoWriteConfiguration : ConfigurationBase, IFormattableConfiguration
    {
        public string RelativePath { get; set; }
        public bool FormatNames { get; set; }
        public bool Test2 { get; set; }
    }
}