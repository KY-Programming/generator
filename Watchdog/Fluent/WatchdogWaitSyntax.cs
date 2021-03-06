﻿using System;
using System.Collections.Generic;
using KY.Core.Dependency;
using KY.Generator.Command;
using KY.Generator.Helpers;
using KY.Generator.Syntax;
using KY.Generator.Watchdog.Commands;

namespace KY.Generator
{
    public class WatchdogWaitSyntax : IWatchdogWaitSyntax, IFluentSyntax
    {
        private readonly WatchdogCommand command;
        
        public IDependencyResolver Resolver { get; }
        public List<IGeneratorCommand> Commands { get; } = new List<IGeneratorCommand>();
        
        public WatchdogWaitSyntax(string url, DependencyResolverReference resolverReference)
        {
            this.Resolver = resolverReference;
            this.command = new WatchdogCommand(this.Resolver);
            this.Commands.Add(this.command);
            this.command.Parameters.Url = url;
        }

        public IWatchdogWaitSyntax Timeout(TimeSpan timeout)
        {
            this.command.Parameters.Timeout = timeout;
            return this;
        }

        public IWatchdogWaitSyntax Delay(TimeSpan delay)
        {
            this.command.Parameters.Delay = delay;
            return this;
        }

        public IWatchdogWaitSyntax Sleep(TimeSpan sleep)
        {
            this.command.Parameters.Sleep = sleep;
            return this;
        }

        public IWatchdogWaitSyntax Tries(int tries)
        {
            this.command.Parameters.Tries = tries;
            return this;
        }

        public IReadFluentSyntax Read()
        {
            FluentSyntax syntax = new FluentSyntax(this.Resolver);
            syntax.Commands = this.command.Commands;
            return syntax;
        }

        public IWriteFluentSyntax Write()
        {
            FluentSyntax syntax = new FluentSyntax(this.Resolver);
            syntax.Commands = this.command.Commands;
            return syntax;
        }
    }
}