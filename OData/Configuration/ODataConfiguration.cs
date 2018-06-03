using KY.Generator.Configuration;

namespace KY.Generator.OData.Configuration
{
    public class ODataConfiguration : ConfigurationBase
    {
        public string Connection { get; set; }
        public bool SkipNamespace { get; set; }
        public ODataEntityDataContext DataContext { get; set; }
        public ODataEntityModels Models { get; set; }
    }
}