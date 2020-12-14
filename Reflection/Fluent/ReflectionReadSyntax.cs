using System;
using KY.Generator.Reflection.Commands;
using KY.Generator.Syntax;

namespace KY.Generator.Reflection.Fluent
{
    public class ReflectionReadSyntax : IReflectionReadAndSwitchToWriteSyntax
    {
        private readonly FluentSyntax syntax;

        public ReflectionReadSyntax(FluentSyntax syntax)
        {
            this.syntax = syntax;
        }

        public IReflectionReadAndSwitchToWriteSyntax FromType<T>()
        {
            Type type = typeof(T);
            ReflectionReadCommand command = new ReflectionReadCommand(this.syntax.Resolver);
            command.Parameters.Assembly = type.Assembly.Location;
            command.Parameters.Namespace = type.Namespace;
            command.Parameters.Name = type.Name;
            this.syntax.Commands.Add(command);
            return this;
        }

        public IWriteFluentSyntax Write()
        {
            return this.syntax;
        }
    }
}