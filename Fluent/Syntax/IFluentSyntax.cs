using System;
using System.Linq.Expressions;
using System.Reflection;

namespace KY.Generator.Syntax
{
    public interface IFluentSyntax<out TSyntax>
    {
        TSyntax SetGlobal(Assembly assembly, Action<ISetFluentSyntax> action);
        TSyntax SetType<T>(Action<ISetFluentSyntax> action);
        TSyntax SetMember<T>(Expression<T> memberAction, Action<ISetFluentSyntax> action);
    }
}
