using System.Reflection;

namespace KY.Generator;

public class OptionsMapping(MethodInfo methodInfo, object?[] arguments)
{
    public MethodInfo MethodInfo { get; } = methodInfo;
    public object?[] Arguments { get; } = arguments;

    public void Deconstruct(out MethodInfo methodInfo, out object?[] arguments)
    {
        methodInfo = this.MethodInfo;
        arguments = this.Arguments;
    }

    public T Execute<T>(Options options) where T : OptionsBase<T>
    {
        MethodInfo genericMethod = this.MethodInfo.MakeGenericMethod(typeof(T));
        object?[] arguments = this.Arguments.ToArray();
        object? secondArgument = this.Arguments[1];
        if (secondArgument != null)
        {
            OptionsMapping? mapping = Options.Resolve(secondArgument);
            if (mapping != null)
            {
                arguments[1] = mapping.Execute<T>(options);
            }
            else
            {
                arguments[1] = options.Get<T>(secondArgument);
            }
        }
        return (T)genericMethod.Invoke(options, arguments);
    }
}