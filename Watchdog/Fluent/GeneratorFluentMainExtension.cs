namespace KY.Generator
{
    public static class GeneratorFluentMainExtension
    {
        public static IWatchdogWaitSyntax WaitFor(this GeneratorFluentMain main, string url)
        {
            WatchdogWaitSyntax syntax = main.Resolver.Create<WatchdogWaitSyntax>(url);
            main.Syntaxes.Add(syntax);
            return syntax;
        }

    }
}
