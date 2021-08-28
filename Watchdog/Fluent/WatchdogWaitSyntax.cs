using System;
using System.Collections.Generic;
using KY.Core.Dependency;
using KY.Generator.Command;
using KY.Generator.Syntax;
using KY.Generator.Watchdog.Commands;

namespace KY.Generator
{
    public class WatchdogWaitSyntax : IWatchdogWaitSyntax, IFluentInternalSyntax, IExecutableSyntax
    {
        private readonly WatchdogCommand command;

        public IDependencyResolver Resolver { get; }
        public List<IGeneratorCommand> Commands { get; } = new();
        public List<IExecutableSyntax> Syntaxes { get; } = new();

        public WatchdogWaitSyntax(string url, IDependencyResolver resolver)
        {
            this.Resolver = resolver;
            this.command = this.Resolver.Create<WatchdogCommand>();
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
            FluentSyntax syntax = this.Resolver.Create<FluentSyntax>();
            syntax.Syntaxes.Add(this);
            return syntax;
        }

        public IWriteFluentSyntax Write()
        {
            FluentSyntax syntax = this.Resolver.Create<FluentSyntax>();
            syntax.Syntaxes.Add(this);
            return syntax;
        }

        public IGeneratorCommandResult Run()
        {
            return new SuccessResult();
        }
    }
}
