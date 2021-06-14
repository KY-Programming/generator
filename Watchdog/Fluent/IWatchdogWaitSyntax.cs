using System;
using KY.Generator.Syntax;

namespace KY.Generator
{
    public interface IWatchdogWaitSyntax
    {
        IWatchdogWaitSyntax Timeout(TimeSpan timeout);
        IWatchdogWaitSyntax Delay(TimeSpan delay);
        IWatchdogWaitSyntax Sleep(TimeSpan sleep);
        IWatchdogWaitSyntax Tries(int tries);
        IReadFluentSyntax Read();
        IWriteFluentSyntax Write();
    }
}