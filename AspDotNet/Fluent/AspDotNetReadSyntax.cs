using System;
using System.Collections.Generic;
using KY.Core.Dependency;
using KY.Generator.AspDotNet.Commands;
using KY.Generator.Command;
using KY.Generator.Syntax;

namespace KY.Generator.AspDotNet.Fluent
{
    internal class AspDotNetReadSyntax : IAspDotNetReadSyntax, IExecutableSyntax
    {
        private readonly IDependencyResolver resolver;

        public List<IGeneratorCommand> Commands { get; } = new();

        public AspDotNetReadSyntax(IDependencyResolver resolver)
        {
            this.resolver = resolver;
        }

        public IAspDotNetReadSyntax FromController<T>()
        {
            Type type = typeof(T);
            AspDotNetReadControllerCommand command = this.resolver.Create<AspDotNetReadControllerCommand>();
            command.Parameters.Assembly = type.Assembly.Location;
            command.Parameters.Namespace = type.Namespace;
            command.Parameters.Name = type.Name;
            this.Commands.Add(command);
            return this;
        }

        public IAspDotNetReadSyntax FromHub<T>()
        {
            Type type = typeof(T);
            AspDotNetReadHubCommand command = this.resolver.Create<AspDotNetReadHubCommand>();
            command.Parameters.Assembly = type.Assembly.Location;
            command.Parameters.Namespace = type.Namespace;
            command.Parameters.Name = type.Name;
            this.Commands.Add(command);
            return this;
        }
    }
}
