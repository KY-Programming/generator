using KY.Generator.Reflection.Commands;
using KY.Generator.Syntax;

namespace KY.Generator.Reflection.Fluent
{
    internal class ReflectionFromTypeSyntax : IReflectionFromTypeOrReflectionReadSyntax
    {
        private readonly ReflectionReadSyntax syntax;
        private readonly ReflectionReadCommand command;

        public ReflectionFromTypeSyntax(ReflectionReadSyntax syntax, ReflectionReadCommand command)
        {
            this.syntax = syntax;
            this.command = command;
        }

        public IReflectionReadSyntax SkipSelf()
        {
            this.command.Parameters.SkipSelf = true;
            return this.syntax;
        }

        public IWriteFluentSyntax Write()
        {
            return this.syntax.Write();
        }

        public IReflectionFromTypeOrReflectionReadSyntax FromType<T>()
        {
            return this.syntax.FromType<T>();
        }
    }
}