using System;
using System.Collections.Generic;
using KY.Core;
using KY.Generator.Configurations;
using KY.Generator.Reflection.Configurations;
using KY.Generator.Transfer;
using KY.Generator.Transfer.Readers;

namespace KY.Generator.Reflection.Readers
{
    internal class ReflectionReader : ITransferReader
    {
        private readonly ReflectionModelReader modelReader;

        public ReflectionReader(ReflectionModelReader modelReader)
        {
            this.modelReader = modelReader;
        }

        public void Read(ConfigurationBase configurationBase, List<ITransferObject> transferObjects)
        {
            ReflectionReadConfiguration configuration = (ReflectionReadConfiguration)configurationBase;
            Type type = GeneratorTypeLoader.Get(configuration, configuration.Assembly, configuration.Namespace, configuration.Name);
            if (type == null)
            {
                Logger.Trace($"Class {configuration.Namespace}.{configuration.Name} not found");
                return;
            }
            ModelTransferObject selfModel = this.modelReader.Read(type, transferObjects);
            if (configuration.SkipSelf)
            {
                transferObjects.Remove(selfModel);
                Logger.Trace($"{selfModel.Name} ({selfModel.Namespace}) skipped through configuration");
            }
        }
    }
}