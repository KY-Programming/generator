using KY.Generator.Configuration;

namespace KY.Generator.OData.Configurations
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