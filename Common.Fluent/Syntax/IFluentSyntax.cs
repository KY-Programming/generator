using System.Linq.Expressions;
using System.Reflection;

namespace KY.Generator;

public interface IFluentSyntax<out TSyntax>
{
    TSyntax SetGlobal(Assembly assembly, Action<ISetFluentSyntax> action);
    TSyntax SetType<T>(Action<ISetFluentSyntax> action);
    TSyntax SetMember<T>(Expression<Func<T, object>> memberExpression, Action<ISetMemberFluentSyntax> action);
    TSyntax SetMember<T>(Expression<Action<T>> memberExpression, Action<ISetMemberFluentSyntax> action);
    TSyntax SetMember<T>(string name, Action<ISetMemberFluentSyntax> action);
}
