using System;
using System.Collections.Generic;
using KY.Core.Dependency;
using KY.Generator.Transfer;

namespace KY.Generator.Configuration
{
    public class WriterConfigurationMapping
    {
        private readonly IDependencyResolver resolver;
        private readonly Dictionary<string, Type> configurationMapping = new Dictionary<string, Type>();
        private readonly Dictionary<string, Type> writerMapping = new Dictionary<string, Type>();
        private readonly Dictionary<Type, Type> readerConfigurationMapping = new Dictionary<Type, Type>();

        public WriterConfigurationMapping(IDependencyResolver resolver)
        {
            this.resolver = resolver;
        }

        public WriterConfigurationMapping Map<TConfiguration, TWriter>(string name)
            where TConfiguration : ConfigurationBase
            where TWriter : ITransferWriter
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            name = name.ToLower();
            this.configurationMapping.Add(name, typeof(TConfiguration));
            this.writerMapping.Add(name, typeof(TWriter));
            this.readerConfigurationMapping.Add(typeof(TConfiguration), typeof(TWriter));
            return this;
        }

        public Type Resolve(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            if (!this.configurationMapping.ContainsKey(name.ToLower()))
            {
                throw new InvalidOperationException($"Writer {name} not found. Please check if the module was loaded");
            }
            return this.configurationMapping[name.ToLower()];
        }

        public ITransferWriter Resolve(ConfigurationBase configuration)
        {
            return (ITransferWriter)this.resolver.Create(this.readerConfigurationMapping[configuration.GetType()]);
        }
    }
}