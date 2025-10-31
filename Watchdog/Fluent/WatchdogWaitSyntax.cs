using KY.Core.Dependency;
using KY.Generator.Command;
using KY.Generator.Syntax;
using KY.Generator.Watchdog.Commands;

namespace KY.Generator;

public class WatchdogWaitSyntax : ExecutableSyntax, IWatchdogWaitSyntax, IFluentInternalSyntax
{
    private readonly WatchdogCommandParameters command;

    public IDependencyResolver Resolver { get; }
    public List<ExecutableSyntax> Syntaxes { get; } = new();

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

    public Task<IGeneratorCommandResult> Run()
    {
        return Task.FromResult<IGeneratorCommandResult>(new SuccessResult());
    }

    public void FollowUp()
    { }
}
