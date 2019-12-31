using System;
using System.Collections.Generic;
using System.Linq;
using KY.Core;
using KY.Core.Dependency;
using KY.Generator.Configuration;
using KY.Generator.Configurations;
using KY.Generator.Output;
using KY.Generator.Transfer;
using KY.Generator.Transfer.Readers;
using KY.Generator.Transfer.Writers;

namespace KY.Generator
{
    public class ConfigurationRunner
    {
        private readonly IDependencyResolver resolver;

        public ConfigurationRunner(IDependencyResolver resolver)
        {
            this.resolver = resolver;
        }

        public bool Run(List<ConfigurationSet> configurations, IOutput output, bool isBeforeBuild = false)
        {
            ConfigurationMapping mapping = this.resolver.Get<ConfigurationMapping>();
            Logger.Trace($"Start generating {configurations.SelectMany(x => x.Configurations).Count(x => x.BeforeBuild == isBeforeBuild)} configurations");
            bool success = true;
            foreach (ConfigurationSet set in configurations)
            {
                List<ConfigurationBase> configurationsToRun = set.Configurations.Where(x => x.BeforeBuild == isBeforeBuild).ToList();
                ConfigurationBase missingLanguage = configurationsToRun.FirstOrDefault(x => x.Language == null && x.RequireLanguage);
                if (missingLanguage != null)
                {
                    Logger.Error($"Configuration '{missingLanguage.GetType().Name}' without language found. Generation failed!");
                    success = false;
                    continue;
                }
                try
                {
                    List<ITransferObject> transferObjects = new List<ITransferObject>();
                    foreach (ConfigurationBase configuration in configurationsToRun)
                    {
                        switch (mapping.Resolve(configuration))
                        {
                            case ITransferReader reader:
                                reader.Read(configuration, transferObjects);
                                break;
                            case ITransferWriter writer:
                                writer.Write(configuration, transferObjects, output);
                                break;
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