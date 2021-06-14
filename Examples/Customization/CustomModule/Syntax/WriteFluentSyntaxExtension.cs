using CustomModule.Commands;
using KY.Generator.Syntax;

namespace CustomModule.Syntax
{
    // 9. Create a Write action
    //  a. Make the class public and static
    //  b. Add a action like Message method
    //  c. Instantiate a command and set parameters
    public static class WriteFluentSyntaxExtension
    {
        public static void Message(this IWriteFluentSyntax syntax, string message, string className, string nameSpace, string relativePath = null)
        {
            FluentSyntax fluentSyntax = (FluentSyntax)syntax;
            WriteCommand command = fluentSyntax.Resolver.Create<WriteCommand>();
            command.Parameters.Message = message;
            command.Parameters.Class = className;
            command.Parameters.Namespace = nameSpace;
            command.Parameters.RelativePath = relativePath;
            fluentSyntax.Commands.Add(command);
        }
    }
}