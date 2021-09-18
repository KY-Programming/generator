using KY.Generator.Reflection.Commands;

namespace KY.Generator.Reflection.Fluent
{
    internal class ReflectionFromTypeSyntax : IReflectionFromTypeSyntax
    {
        private readonly ReflectionReadCommand command;

        public ReflectionFromTypeSyntax(ReflectionReadCommand command)
        {
            this.command = command;
        }

        public IReflectionFromTypeSyntax OnlySubTypes()
        {
            this.command.Parameters.OnlySubTypes = true;
            return this;
        }
    }
}
