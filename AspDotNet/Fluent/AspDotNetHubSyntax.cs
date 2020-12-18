using KY.Generator.AspDotNet.Commands;
using KY.Generator.Syntax;

namespace KY.Generator.AspDotNet.Fluent
{
    internal class AspDotNetHubSyntax : IAspDotNetHubOrReadSyntax
    {
        private readonly AspDotNetReadSyntax syntax;
        private readonly AspDotNetReadHubCommand command;

        public AspDotNetHubSyntax(AspDotNetReadSyntax syntax, AspDotNetReadHubCommand command)
        {
            this.syntax = syntax;
            this.command = command;
        }

        public IAspDotNetControllerOrReadSyntax FromController<T>()
        {
            return this.syntax.FromController<T>();
        }

        public IAspDotNetHubOrReadSyntax FromHub<T>()
        {
            return this.syntax.FromHub<T>();
        }

        public IWriteFluentSyntax Write()
        {
            return this.syntax.Write();
        }
    }
}