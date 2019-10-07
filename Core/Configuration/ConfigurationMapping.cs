using System;
using System.Collections.Generic;
using KY.Core.Dependency;
using KY.Generator.Transfer;

namespace KY.Generator.Configuration
{
    public class ReaderConfigurationMapping
    {
        private readonly IDependencyResolver resolver;
        private readonly Dictionary<string, Type> configurationMapping = new Dictionary<string, Type>();
        private readonly Dictionary<string, Type> readerMapping = new Dictionary<string, Type>();
        private readonly Dictionary<Type, Type> readerConfigurationMapping = new Dictionary<Type, Type>();

        public ReaderConfigurationMapping(IDependencyResolver resolver)
        {
            this.resolver = resolver;
        }

        public ReaderConfigurationMapping Map<TConfiguration, TReader>(string name)
            where TConfiguration : ConfigurationBase
            where TReader : ITransferReader
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            name = name.ToLower();
            this.configurationMapping.Add(name, typeof(TConfiguration));
            this.readerMapping.Add(name, typeof(TReader));
            this.readerConfigurationMapping.Add(typeof(TConfiguration), typeof(TReader));
            return this;
        }

        public Type Resolve(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            return this.configurationMapping[name.ToLower()];
        }

        public ITransferReader Resolve(ConfigurationBase configuration)
        {
            return (ITransferReader)this.resolver.Create(this.readerConfigurationMapping[configuration.GetType()]);
        }
    }
}