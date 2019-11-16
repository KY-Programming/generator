using KY.Generator.Configuration;
using KY.Generator.Configurations;

namespace KY.Generator.Client
{
    internal class GeneratorConfiguration : ConfigurationBase
    {
        public GeneratorClientConfiguration Client { get;set; }
    }
}