using System;
using System.Collections.Generic;
using KY.Core;
using KY.Generator.Command;
using KY.Generator.Configuration;
using KY.Generator.Reflection.Configurations;
using KY.Generator.Reflection.Readers;
using KY.Generator.Transfer;

namespace KY.Generator.Reflection.Commands
{
    internal class ReadReflectionCommand : IConfigurationCommand
    {
        private readonly ReflectionModelReader modelReader;

        public ReadReflectionCommand(ReflectionModelReader modelReader)
        {
            this.modelReader = modelReader;
        }

        public bool Execute(IConfiguration configurationBase, List<ITransferObject> transferObjects)
        {
            Logger.Trace("Read reflection...");
            ReadReflectionConfiguration configuration = (ReadReflectionConfiguration)configurationBase;
            Type type = GeneratorTypeLoader.Get(configuration, configuration.Assembly, configuration.Namespace, configuration.Name);
            if (type != null)
            {
                ModelTransferObject selfModel = this.modelReader.Read(type, transferObjects);
                if (configuration.SkipSelf)
                {
                    transferObjects.Remove(selfModel);
                    Logger.Trace($"{selfModel.Name} ({selfModel.Namespace}) skipped through configuration");
                }
            }
            return true;
        }
    }
}