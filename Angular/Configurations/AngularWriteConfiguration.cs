using KY.Generator.Configuration;
using KY.Generator.TypeScript.Languages;

namespace KY.Generator.Angular.Configurations
{
    public class AngularWriteConfiguration : ConfigurationBase
    {
        public AngularWriteServiceConfiguration Service { get; set; }
        public AngularWriteModelConfiguration Model { get; set; }
        public bool FormatNames { get; set; }
        public bool WriteModels { get; set; }

        public AngularWriteConfiguration()
        {
            this.Language = TypeScriptLanguage.Instance;
            this.FormatNames = true;
            this.WriteModels = true;
        }
    }
}