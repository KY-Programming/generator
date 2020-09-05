using System;
using System.Collections.Generic;
using System.Linq;
using KY.Generator.Configurations;
using KY.Generator.Languages;

namespace KY.Generator.Command.Extensions
{
    public static class ConfigurationBaseExtension
    {
        public static void ReadFromParameters(this ConfigurationBase configuration, List<ICommandParameter> parameters, List<ILanguage> languages = null)
        {
            configuration.VerifySsl = parameters.GetBool(nameof(ConfigurationBase.VerifySsl), configuration.VerifySsl);
            if (languages != null)
            {
                configuration.Language = languages.FirstOrDefault(x => x.Name.Equals(parameters.GetString(nameof(ConfigurationBase.Language)), StringComparison.InvariantCultureIgnoreCase)) ?? configuration.Language;
            }
            configuration.AddHeader = parameters.GetBool(nameof(ConfigurationBase.AddHeader), configuration.AddHeader);
            configuration.AddHeader = !parameters.Exists("skipHeader") && configuration.AddHeader;
        }
    }
}