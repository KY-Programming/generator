using System.Collections.Generic;
using KY.Generator.Configuration;
using KY.Generator.Configurations;

namespace KY.Generator.Extensions
{
    public static class ConfigurationListExtension
    {
        public static IEnumerable<IConfiguration> Flatten(this IEnumerable<IConfiguration> configurations)
        {
            foreach (IConfiguration configuration in configurations)
            {
                yield return configuration;
                if (configuration is ExecuteConfiguration executeConfiguration)
                {
                    foreach (IConfiguration child in executeConfiguration.Execute.Flatten())
                    {
                        yield return child;
                    }
                }
            }
        }
    }
}