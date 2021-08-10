using System;

namespace KY.Generator.Syntax
{
    public interface IReadFluentSyntax
    {
        IReadFluentSyntax SetType<T>(Action<ISetFluentSyntax> action);
    }
}
