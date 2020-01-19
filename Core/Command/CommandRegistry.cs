using System;
using System.Collections.Generic;
using System.Linq;
using KY.Core;
using KY.Core.Dependency;
using KY.Generator.Configuration;
using KY.Generator.Exceptions;

namespace KY.Generator.Command
{
    public class CommandRegistry
    {
        public const string DefaultGroup = "execute";
        private readonly IDependencyResolver resolver;
        private readonly List<RegistryEntry> entries;

        public CommandRegistry(IDependencyResolver resolver)
        {
            this.resolver = resolver.AssertIsNotNull(nameof(resolver));
            this.entries = new List<RegistryEntry>();
        }

        public IRegisterSyntax Register<TCommand, TConfiguration>(string name, string group = DefaultGroup)
            where TCommand : ICommand
            where TConfiguration : IConfiguration
        {
            group = string.IsNullOrWhiteSpace(group) ? DefaultGroup : group;
            if (this.entries.Any(entry => entry.Group.Equals(group, StringComparison.InvariantCultureIgnoreCase) && entry.HasName(name)))
            {
                throw new CommandAlreadyRegisteredException(name, group);
            }
            RegistryEntry commandUsesConfiguration = this.entries.FirstOrDefault(entry => entry.Configuration == typeof(TConfiguration));
            if (commandUsesConfiguration != null)
            {
                throw new AmbiguousConfigurationException(typeof(TConfiguration), commandUsesConfiguration.Name, commandUsesConfiguration.Group);
            }
            RegistryEntry registryEntry = new RegistryEntry
                                          {
                                              Name = name,
                                              Group = group,
                                              Command = typeof(TCommand),
                                              Configuration = typeof(TConfiguration)
                                          };
            this.entries.Add(registryEntry);
            return new RegisterSyntax(this, registryEntry);
        }

        private RegistryEntry Find(string command, string group = null)
        {
            List<RegistryEntry> found;
            if (!string.IsNullOrEmpty(group))
            {
                found = this.entries.Where(entry => entry.HasName(command)
                                                    && entry.Group.Equals(group, StringComparison.CurrentCultureIgnoreCase))
                            .ToList();
            }
            else
            {
                found = this.entries.Where(entry => entry.HasName(command) && entry.Group == DefaultGroup).ToList();
                if (found.Count == 0)
                {
                    found = this.entries.Where(entry => entry.HasName(command)).ToList();
                }
                if (found.Count > 1)
                {
                    throw new AmbiguousCommandException($"Ambiguous command '{command}'. Use {string.Join(" or ", found.Select(x => $"'{x.Group}-{x.Name}'"))} instead.");
                }
                if (found.Count == 0)
                {
                    found = this.entries.Where(entry => $"{entry.Group}-{entry.Name}".Equals(command, StringComparison.InvariantCultureIgnoreCase)
                                                        || entry.Aliases.Any(alias => $"{entry.Group}-{alias}".Equals(command, StringComparison.InvariantCultureIgnoreCase))
                    ).ToList();
                }
            }
            if (found.Count == 0)
            {
                throw new CommandNotFoundException(command);
            }
            return found.Single();
        }

        public Type GetConfigurationType(string command, string group = null)
        {
            return this.Find(command, group)?.Configuration;
        }

        public IConfiguration CreateConfiguration(string command, string group = null)
        {
            Type type = this.GetConfigurationType(command, group);
            if (type == null)
            {
                return null;
            }
            IConfiguration configuration = (IConfiguration)this.resolver.Create(type);
            if (configuration.Environment == null)
            {
                throw new CommandWithoutEnvironmentException(configuration.GetType());
            }
            configuration.Environment.Command = command;
            configuration.Environment.CommandGroup = group;
            return configuration;
        }

        public ICommand CreateCommand(IConfiguration configuration)
        {
            Type type = configuration.GetType();
            RegistryEntry entry = this.entries.FirstOrDefault(x => x.Configuration == type);
            return entry == null ? null : (ICommand)this.resolver.Create(entry.Command);
        }

        public List<string> GetGroups()
        {
            return this.entries.Select(x => x.Group).Unique().ToList();
        }

        public interface IRegisterSyntax
        {
            IRegisterSyntax Register<TCommand, TConfiguration>(string name, string group = DefaultGroup)
                where TCommand : ICommand
                where TConfiguration : IConfiguration;

            IRegisterSyntax Alias(params string[] aliases);
        }

        private class RegisterSyntax : IRegisterSyntax
        {
            private readonly CommandRegistry commandRegistry;
            private readonly RegistryEntry entry;

            public RegisterSyntax(CommandRegistry commandRegistry, RegistryEntry entry)
            {
                this.commandRegistry = commandRegistry;
                this.entry = entry;
            }

            public IRegisterSyntax Register<TCommand, TConfiguration>(string name, string group = DefaultGroup)
                where TCommand : ICommand
                where TConfiguration : IConfiguration
            {
                return this.commandRegistry.Register<TCommand, TConfiguration>(name, group);
            }

            public IRegisterSyntax Alias(params string[] aliases)
            {
                this.entry.Aliases.AddRange(aliases);
                return this;
            }
        }

        private class RegistryEntry
        {
            public string Name { get; set; }
            public string Group { get; set; }
            public Type Command { get; set; }
            public Type Configuration { get; set; }
            public List<string> Aliases { get; } = new List<string>();

            public bool HasName(string name)
            {
                return this.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase)
                       || this.Aliases.Any(alias => alias.Equals(name, StringComparison.InvariantCultureIgnoreCase));
            }
        }
    }
}