using System;
using KY.Generator.AspDotNet.Commands;
using KY.Generator.Syntax;

namespace KY.Generator.AspDotNet.Fluent
{
    internal class AspDotNetReadSyntax : IAspDotNetReadSyntax
    {
        private readonly FluentSyntax syntax;

        public AspDotNetReadSyntax(FluentSyntax syntax)
        {
            this.syntax = syntax;
        }

        public IAspDotNetControllerOrReadSyntax FromController<T>()
        {
            Type type = typeof(T);
            AspDotNetReadControllerCommand command = new AspDotNetReadControllerCommand(this.syntax.Resolver);
            command.Parameters.Assembly = type.Assembly.Location;
            command.Parameters.Namespace = type.Namespace;
            command.Parameters.Name = type.Name;
            this.syntax.Commands.Add(command);
            return new AspDotNetControllerSyntax(this, command);
        }

        public IAspDotNetHubOrReadSyntax FromHub<T>()
        {
            Type type = typeof(T);
            AspDotNetReadHubCommand command = new AspDotNetReadHubCommand(this.syntax.Resolver);
            command.Parameters.Assembly = type.Assembly.Location;
            command.Parameters.Namespace = type.Namespace;
            command.Parameters.Name = type.Name;
            this.syntax.Commands.Add(command);
            return new AspDotNetHubSyntax(this, command);
        }

        public IWriteFluentSyntax Write()
        {
            return this.syntax;
        }
    }
}