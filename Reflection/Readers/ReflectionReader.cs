using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
            this.modelReader.Read(this.LoadType(configuration)).ForEach(transferObjects.Add);
        }

        private Type LoadType(ReflectionReadConfiguration reflectionType)
        {
            if (string.IsNullOrEmpty(reflectionType.Namespace))
            {
                Logger.Error("Reflection: Namespace can not be empty");
                return null;
            }
            if (string.IsNullOrEmpty(reflectionType.Name))
            {
                Logger.Error("Reflection: Name can not be empty");
                return null;
            }
            string name = $"{reflectionType.Namespace}.{reflectionType.Name}";
            if (string.IsNullOrEmpty(reflectionType.Assembly))
            {
                return AppDomain.CurrentDomain.GetAssemblies().Select(x => x.GetType(name)).First(x => x != null);
            }
            Assembly assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(x => x.GetName().Name == reflectionType.Assembly);
            if (assembly == null)
            {
                assembly = Assembly.LoadFrom(reflectionType.Assembly);
            }
            //if (assembly == null)
            //{
            //    Logger.Error($"Reflection: Assembly {reflectionType.Assembly} not found");
            //    return null;
            //}
            Type type = assembly.GetType(name);
            if (type == null)
            {
                Logger.Error($"Reflection: {name} not found in {assembly.GetName().Name}");
                return null;
            }
            return type;
        }
    }
}