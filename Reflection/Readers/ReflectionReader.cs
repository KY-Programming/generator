using System;
using System.Collections.Generic;
using KY.Core;
using KY.Generator.Configuration;
using KY.Generator.Reflection.Configuration;
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
            if (type != null)
            {
                this.modelReader.Read(type).ForEach(transferObjects.Add);
            }
        }
    }
}