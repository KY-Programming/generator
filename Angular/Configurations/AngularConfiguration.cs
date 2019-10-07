using KY.Generator.Configuration;
using KY.Generator.TypeScript.Languages;

namespace KY.Generator.Angular.Configurations
{
    internal class AngularConfiguration : ConfigurationBase
    {
        public AngularServiceConfiguration Service { get; set; }
        public bool FormatNames { get; set; }

        public AngularConfiguration()
        {
            this.Language = TypeScriptLanguage.Instance;
            this.FormatNames = true;
        }
    }
}