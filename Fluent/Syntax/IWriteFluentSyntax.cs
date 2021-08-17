using System;
using System.Linq.Expressions;
using System.Reflection;

namespace KY.Generator.Syntax
{
    public interface IWriteFluentSyntax
    {
        IWriteFluentSyntax SetGlobal(Assembly assembly, Action<ISetFluentSyntax> action);
        IWriteFluentSyntax SetType<T>(Action<ISetFluentSyntax> action);
        IWriteFluentSyntax SetMember<T>(Expression<T> memberAction, Action<ISetFluentSyntax> action);
    }
}
