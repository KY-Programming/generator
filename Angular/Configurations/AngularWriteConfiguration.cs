using KY.Generator.Angular.Languages;
using KY.Generator.Configurations;

namespace KY.Generator.Angular.Configurations
{
    public class AngularWriteConfiguration : ConfigurationBase, IFormattableConfiguration
    {
        public AngularWriteServiceConfiguration Service { get; set; }
        public AngularWriteModelConfiguration Model { get; set; }
        public bool FormatNames { get; set; }
        public bool WriteModels { get; set; }

        public AngularWriteConfiguration(ConfigurationBase copyFrom = null)
        {
            if (copyFrom != null)
            {
                this.CopyBaseFrom(copyFrom);
            }
            this.Language = new AngularTypeScriptLanguage();
            this.FormatNames = true;
            this.WriteModels = true;
            this.Model = new AngularWriteModelConfiguration();
        }
    }
}