using KY.Generator.Angular.Languages;
using KY.Generator.Configuration;

namespace KY.Generator.Angular.Configurations
{
    public class AngularWriteConfiguration : ConfigurationWithLanguageBase, IFormattableConfiguration
    {
        public AngularWriteServiceConfiguration Service { get; set; }
        public AngularWriteModelConfiguration Model { get; set; }
        public bool FormatNames { get; set; }
        public bool WriteModels { get; set; }

        public override bool AddHeader
        {
            get => this.Model.AddHeader;
            set => this.Model.AddHeader = value;
        }

        public AngularWriteConfiguration()
        {
            this.Language = new AngularTypeScriptLanguage();
            this.FormatNames = true;
            this.WriteModels = true;
            this.Model = new AngularWriteModelConfiguration();
        }
    }
}