using System;

namespace KY.Generator.Configuration
{
    public class ConfigurationMappingEntry
    {
        public string Action { get; }
        public string Name { get; }
        public Type Configuration { get; }
        public Type Actor { get; }

        public ConfigurationMappingEntry(string action, string name, Type configuration, Type actor)
        {
            this.Action = action;
            this.Name = name;
            this.Configuration = configuration;
            this.Actor = actor;
        }
    }
}