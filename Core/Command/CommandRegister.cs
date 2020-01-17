using System;
using System.Collections.Generic;
using System.Linq;
using KY.Core;
using KY.Core.Dependency;
using KY.Generator.Configurations;
using KY.Generator.Exceptions;

namespace KY.Generator.Command
{
    public class CommandRegister
    {
        public const string DefaultGroup = "execute";
        private readonly IDependencyResolver resolver;
        private readonly List<RegisterEntry> entries;

        public CommandRegister(IDependencyResolver resolver)
        {
            this.resolver = resolver.AssertIsNotNull(nameof(resolver));
            this.entries = new List<RegisterEntry>();
        }

        public CommandRegister Register<TCommand, TConfiguration>(string name, string group = DefaultGroup)
            where TCommand : IGeneratorCommand
            where TConfiguration : IConfiguration
        {
            group = string.IsNullOrWhiteSpace(group) ? DefaultGroup : group;
            if (this.entries.Any(entry => entry.Group.Equals(group, StringComparison.InvariantCultureIgnoreCase) && entry.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase)))
            {
                throw new InvalidOperationException($"{name} ({group}) is already mapped. Please use a prefix like my-{name}");
            }
            this.entries.Add(new RegisterEntry
                             {
                                 Name = name,
                                 Group = group,
                                 Command = typeof(TCommand),
                                 Configuration = typeof(TConfiguration)
                             });
            return this;
        }

        private RegisterEntry Find(string command)
        {
            List<RegisterEntry> found = this.entries.Where(entry => entry.Name.Equals(command, StringComparison.InvariantCultureIgnoreCase)).ToList();
            if (found.Count > 1)
            {
                throw new AmbiguousCommandException($"Ambiguous command '{command}'. Use {string.Join(" or ", found.Select(x => $"'{x.Group}-{x.Name}'"))} instead.");
            }
            if (found.Count == 0)
            {
                found = this.entries.Where(entry => $"{entry.Group}-{entry.Name}".Equals(command, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }
            if (found.Count == 0)
            {
                throw new CommandNotFoundException(command);
            }
            return found.Single();
        }

        public IConfiguration CreateConfiguration(string command)
        {
            RegisterEntry entry = this.Find(command);
            return entry == null ? null : this.resolver.Create(entry.Configuration) as IConfiguration;
        }

        public IGeneratorCommand CreateCommand(IConfiguration configuration)
        {
            Type type = configuration.GetType();
            RegisterEntry entry = this.entries.FirstOrDefault(x => x.Configuration == type);
            return entry == null ? null : this.resolver.Create(entry.Command) as IGeneratorCommand;
        }

        private class RegisterEntry
        {
            public string Name { get; set; }
            public string Group { get; set; }
            public Type Command { get; set; }
            public Type Configuration { get; set; }
        }
    }
}