using System;

namespace KY.Generator.Syntax
{
    public interface IWriteFluentSyntax : IFluentSyntax<IWriteFluentSyntax>
    {
        IWriteFluentSyntax Formatting(Action<IFormattingFluentSyntax> action);
    }
}
