using KY.Generator.Configurations;

namespace KY.Generator.OData.Configuration
{
    public class ODataReadConfiguration : ConfigurationBase
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