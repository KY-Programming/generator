using KY.Generator.Configurations;
using KY.Generator.TypeScript.Languages;

namespace KY.Generator.Angular.Configurations
{
    public class AngularWriteModelConfiguration : ModelWriteConfiguration
    {
        public AngularWriteModelConfiguration()
        {
            this.Language = TypeScriptLanguage.Instance;
            this.SkipNamespace = true;
        }
    }
}
