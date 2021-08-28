using System;
using System.Collections.Generic;
using KY.Generator.AspDotNet.Commands;
using KY.Generator.Command;
using KY.Generator.Syntax;

namespace KY.Generator.AspDotNet.Fluent
{
    internal class AspDotNetReadSyntax : IAspDotNetReadSyntax, IExecutableSyntax
    {
        private readonly IReadFluentSyntaxInternal syntax;

        public List<IGeneratorCommand> Commands { get; } = new();

        public AspDotNetReadSyntax(IReadFluentSyntaxInternal syntax)
        {
            this.syntax = syntax;
        }

        public IAspDotNetControllerOrReadSyntax FromController<T>()
        {
            Type type = typeof(T);
            AspDotNetReadControllerCommand command = this.syntax.Resolver.Create<AspDotNetReadControllerCommand>();
            command.Parameters.Assembly = type.Assembly.Location;
            command.Parameters.Namespace = type.Namespace;
            command.Parameters.Name = type.Name;
            this.Commands.Add(command);
            return new AspDotNetControllerSyntax(this, command);
        }

        public IAspDotNetHubOrReadSyntax FromHub<T>()
        {
            Type type = typeof(T);
            AspDotNetReadHubCommand command = this.syntax.Resolver.Create<AspDotNetReadHubCommand>();
            command.Parameters.Assembly = type.Assembly.Location;
            command.Parameters.Namespace = type.Namespace;
            command.Parameters.Name = type.Name;
            this.Commands.Add(command);
            return new AspDotNetHubSyntax(this, command);
        }

        public IWriteFluentSyntax Write()
        {
            return this.syntax.Write();
        }
    }
}
