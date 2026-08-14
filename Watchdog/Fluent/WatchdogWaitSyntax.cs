using KY.Core.Dependency;
using KY.Generator.Command;
using KY.Generator.Watchdog.Commands;

namespace KY.Generator;

public class WatchdogWaitSyntax : IExecutableSyntax, IWatchdogWaitSyntax, IFluentInternalSyntax
{
    private readonly WatchdogCommandParameters command;

    /// <summary>
    /// Everything declared behind the wait. It holds this syntax as its first entry, so the wait is the
    /// first command of the chain and the read and write commands run behind it.
    /// </summary>
    private FluentSyntax? chain;

    public List<GeneratorCommandParameters> Commands { get; } = [];
    public IDependencyResolver Resolver { get; }
    public List<IExecutableSyntax> Syntaxes { get; } = new();

    public WatchdogWaitSyntax(string url, IDependencyResolver resolver)
    {
        this.Resolver = resolver;
        this.command = new WatchdogCommandParameters();
        this.Commands.Add(this.command);
        this.command.Url = url;
    }

    public IWatchdogWaitSyntax Timeout(TimeSpan timeout)
    {
        this.command.Timeout = timeout;
        return this;
    }

    public IWatchdogWaitSyntax Delay(TimeSpan delay)
    {
        this.command.Delay = delay;
        return this;
    }

    public IWatchdogWaitSyntax Sleep(TimeSpan sleep)
    {
        this.command.Sleep = sleep;
        return this;
    }

    public IWatchdogWaitSyntax Tries(int tries)
    {
        this.command.Tries = tries;
        return this;
    }

    public ISwitchToWriteFluentSyntax Read(Action<IReadFluentSyntax> action)
    {
        return this.Chain().Read(action);
    }

    public void Write(Action<IWriteFluentSyntax> action)
    {
        this.Chain().Write(action);
    }

    public async Task<IGeneratorCommandResult> Run()
    {
        if (this.chain != null)
        {
            return await this.chain.Run();
        }
        // Nothing was declared behind the wait, so there is no chain to run it as part of - but the wait
        // itself still has to happen, otherwise a bare WaitFor(...) would silently do nothing.
        GeneratorCommandRunner runner = this.Resolver.Create<GeneratorCommandRunner>();
        return await runner.Run(runner.Create(this.Commands, this.Resolver));
    }

    public void FollowUp()
    {
        this.chain?.FollowUp();
    }

    private FluentSyntax Chain()
    {
        if (this.chain == null)
        {
            this.chain = this.Resolver.Create<FluentSyntax>();
            this.chain.Syntaxes.Add(this);
        }
        return this.chain;
    }
}
