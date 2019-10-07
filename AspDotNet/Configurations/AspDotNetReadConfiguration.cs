using KY.Generator.Configuration;

namespace KY.Generator.AspDotNet.Configurations
{
    internal class AspDotNetReadConfiguration : ConfigurationBase
    {
        public AspDotNetReadControllerConfiguration Controller { get; set; }

        public override bool RequireLanguage => false;
    }
}