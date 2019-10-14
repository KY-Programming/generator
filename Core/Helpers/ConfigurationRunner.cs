using System;
using System.Collections.Generic;
using System.Linq;
using KY.Core;
using KY.Core.Dependency;
using KY.Generator.Configuration;
using KY.Generator.Output;
using KY.Generator.Transfer;

namespace KY.Generator
{
    public class ConfigurationRunner
    {
        private readonly IDependencyResolver resolver;

        public ConfigurationRunner(IDependencyResolver resolver)
        {
            this.resolver = resolver;
        }

        public bool Run(List<ConfigurationPair> configurations, IOutput output)
        {
            ReaderConfigurationMapping readers = this.resolver.Get<ReaderConfigurationMapping>();
            WriterConfigurationMapping writers = this.resolver.Get<WriterConfigurationMapping>();
            Logger.Trace($"Start generating {configurations.Count} configurations");
            bool success = true;
            foreach (ConfigurationPair pair in configurations)
            {
                ConfigurationBase missingLanguage = pair.Writers.FirstOrDefault(x => x.Language == null && x.RequireLanguage);
                if (missingLanguage != null)
                {
                    Logger.Trace($"Configuration '{missingLanguage.GetType().Name}' without language found. Generation failed!");
                    success = false;
                    continue;
                }
                try
                {
                    List<ITransferObject> transferObjects = new List<ITransferObject>();
                    foreach (ConfigurationBase configuration in pair.Readers)
                    {
                        if (readers.Resolve(configuration) is ITransferReader reader)
                        {
                            reader.Read(configuration, transferObjects);
                        }
                    }
                    foreach (ConfigurationBase configuration in pair.Writers)
                    {
                        if (writers.Resolve(configuration) is ITransferWriter writer)
                        {
                            writer.Write(configuration, transferObjects, output);
                        }
                    }
                }
                catch (Exception exception)
                {
                    Logger.Error(exception);
                    success = false;
                }
            }
            return success;
        }
    }
}