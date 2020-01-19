using KY.Generator.Languages;
using Newtonsoft.Json;

namespace KY.Generator.Configuration
{
    public abstract class ConfigurationWithLanguageBase : ConfigurationBase, IConfigurationWithLanguage
    {
        [JsonIgnore]
        [ConfigurationIgnore]
        public virtual ILanguage Language { get; set; }

        [JsonProperty("Language")]
        [ConfigurationProperty("Language")]
        public virtual string LanguageKey { get; set; }

        public override void CopyBaseFrom(IConfiguration source)
        {
            base.CopyBaseFrom(source);
            if (source is IConfigurationWithLanguage configurationWithLanguage)
            {
                this.Language = configurationWithLanguage.Language;
            }
        }
    }
}