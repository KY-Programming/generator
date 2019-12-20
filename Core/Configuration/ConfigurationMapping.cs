using System;
using System.Collections.Generic;
using System.Linq;
using KY.Core;
using KY.Core.Dependency;
using KY.Generator.Configurations;
using KY.Generator.Transfer.Readers;
using KY.Generator.Transfer.Writers;

namespace KY.Generator.Configuration
{
    public class ConfigurationMapping
    {
        private readonly IDependencyResolver resolver;
        private readonly List<ConfigurationMappingEntry> mapping;

        public ConfigurationMapping(IDependencyResolver resolver)
        {
            this.resolver = resolver;
            this.mapping = new List<ConfigurationMappingEntry>();
        }

        public void Map<TConfiguration, TActor>(string name, string action)
            where TConfiguration : IConfiguration
        {
            name.AssertIsNotNull(nameof(name));
            if (this.mapping.Any(entry => entry.Action.Equals(action, StringComparison.InvariantCultureIgnoreCase) && entry.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase)))
            {
                throw new InvalidOperationException($"{name} is already mapped. Please use a prefix like my-{name}");
            }
            if (this.mapping.Any(entry => entry.Action.Equals(action, StringComparison.InvariantCultureIgnoreCase) && entry.Configuration == typeof(TConfiguration)))
            {
                throw new InvalidOperationException($"{typeof(TConfiguration)} is already mapped. Please create a derived class");
            }
            this.mapping.Add(new ConfigurationMappingEntry(action, name.ToLowerInvariant(), typeof(TConfiguration), typeof(TActor)));
        }

        public ConfigurationMapping Map<TConfiguration, TActor>(string name)
            where TConfiguration : IConfiguration
        {
            bool isReader = typeof(ITransferReader).IsAssignableFrom(typeof(TActor));
            bool isWriter = typeof(ITransferWriter).IsAssignableFrom(typeof(TActor));
            if (!isReader && !isWriter)
            {
                throw new InvalidOperationException($"{name} has to be at least a {nameof(ITransferReader)} or {nameof(ITransferWriter)}");
            }
            if (isReader)
            {
                this.Map<TConfiguration, TActor>(name, "read");
            }
            if (isWriter)
            {
                this.Map<TConfiguration, TActor>(name, "write");
            }
            return this;
        }

        public Type ResolveConfiguration(string name, string action)
        {
            action.AssertIsNotNull(nameof(action));
            name.AssertIsNotNull(nameof(name));
            ConfigurationMappingEntry entry = this.mapping.FirstOrDefault(e => e.Action.Equals(action, StringComparison.InvariantCultureIgnoreCase)
                                                                               && e.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
            if (entry == null)
            {
                throw new InvalidOperationException($"{action.FirstCharToUpper()} {name} not found. Please check if the module was loaded");
            }
            return entry.Configuration;
        }

        public object Resolve(IConfiguration configuration)
        {
            configuration.AssertIsNotNull(nameof(configuration));
            ConfigurationMappingEntry entry = this.mapping.FirstOrDefault(e => e.Configuration == configuration.GetType());
            if (entry == null)
            {
                throw new InvalidOperationException($"Actor for {configuration.GetType().Name} not found. Please check if the module was loaded");
            }
            return this.resolver.Create(entry.Actor);
        }

        public List<string> GetActions()
        {
            return this.mapping.Select(x => x.Action).Unique().ToList();
        }
    }
}