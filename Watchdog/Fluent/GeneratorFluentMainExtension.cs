namespace KY.Generator
{
    public static class GeneratorFluentMainExtension
    {
        public static IWatchdogWaitSyntax WaitFor(this GeneratorFluentMain main, string url)
        {
            WatchdogWaitSyntax syntax = new WatchdogWaitSyntax(url, main.ResolverReference);
            main.Syntaxes.Add(syntax);
            return syntax;
        }

    }
}