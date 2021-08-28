using System;
using System.Collections.Generic;
using KY.Generator.Command;
using KY.Generator.Reflection.Commands;
using KY.Generator.Syntax;

namespace KY.Generator.Reflection.Fluent
{
    internal class ReflectionReadSyntax : IReflectionReadSyntax, IExecutableSyntax
    {
        private readonly IReadFluentSyntaxInternal syntax;

        public List<IGeneratorCommand> Commands { get; } = new();

        public ReflectionReadSyntax(IReadFluentSyntaxInternal syntax)
        {
            this.syntax = syntax;
        }

        public IReflectionFromTypeOrReflectionReadSyntax FromType<T>()
        {
            Type type = typeof(T);
            ReflectionReadCommand command = this.syntax.Resolver.Create<ReflectionReadCommand>();
            command.Parameters.Assembly = type.Assembly.Location;
            command.Parameters.Namespace = type.Namespace;
            command.Parameters.Name = type.Name;
            this.Commands.Add(command);
            return new ReflectionFromTypeSyntax(this, command);
        }

        public IWriteFluentSyntax Write()
        {
            return this.syntax.Write();
        }
    }
}
