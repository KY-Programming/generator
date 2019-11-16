using KY.Generator.Configuration;
using KY.Generator.Configurations;

namespace KY.Generator.AspDotNet.Configurations
{
    public class AspDotNetReadConfiguration : ConfigurationBase
    {
        public AspDotNetReadControllerConfiguration Controller { get; set; }

        public override bool RequireLanguage => false;
    }
}