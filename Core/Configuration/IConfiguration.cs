using KY.Generator.Output;

namespace KY.Generator.Configuration
{
    public interface IConfiguration
    {
        ConfigurationFormatting Formatting { get; }
        IOutput Output { get; set; }
        ConfigurationEnvironment Environment { get; }
        bool BeforeBuild { get; set; }
    }
}