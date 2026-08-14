namespace KY.Generator;

public interface IWatchdogWaitSyntax
{
    IWatchdogWaitSyntax Timeout(TimeSpan timeout);
    IWatchdogWaitSyntax Delay(TimeSpan delay);
    IWatchdogWaitSyntax Sleep(TimeSpan sleep);
    IWatchdogWaitSyntax Tries(int tries);

    /// <inheritdoc cref="ISwitchToReadFluentSyntax.Read"/>
    ISwitchToWriteFluentSyntax Read(Action<IReadFluentSyntax> action);

    /// <inheritdoc cref="ISwitchToWriteFluentSyntax.Write"/>
    void Write(Action<IWriteFluentSyntax> action);
}
