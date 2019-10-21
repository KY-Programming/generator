using System;
using System.Collections.Generic;
using System.Linq;
using KY.Generator.Configuration;
using KY.Generator.Languages;

namespace KY.Generator.Command.Extensions
{
    public static class ConfigurationBaseExtension
    {
        public static void ReadFromParameters(this ConfigurationBase configuration, List<CommandParameter> parameters, List<ILanguage> languages)
        {
            configuration.VerifySsl = parameters.GetBool(nameof(ConfigurationBase.VerifySsl), configuration.VerifySsl);
            configuration.Language = languages.FirstOrDefault(x => x.Name.Equals(parameters.GetString(nameof(ConfigurationBase.Language)), StringComparison.InvariantCultureIgnoreCase))?? configuration.Language;
            configuration.AddHeader = parameters.GetBool(nameof(ConfigurationBase.AddHeader), configuration.AddHeader);
            configuration.AddHeader = !parameters.Exists("skipHeader") && configuration.AddHeader;
        }
    }
}