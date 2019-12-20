using KY.Generator.Configurations;

namespace KY.Generator.OData.Configurations
{
    public class ODataReadConfiguration : ReadConfigurationBase
    {
        public string File { get; set; }
        public string Connection { get; set; }
        public bool FormatNames { get; set; }

        public ODataReadConfiguration()
        {
            this.FormatNames = true;
        }
    }
}