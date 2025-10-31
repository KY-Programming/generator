using System.Linq.Expressions;
using System.Reflection;
using KY.Core;
using KY.Generator.Transfer;

namespace KY.Generator;

public class Options
{
    public const string RootKey = "<root>";
    private readonly Dictionary<object, Dictionary<Type, object>> cache = new();
    private static readonly Dictionary<object, Dictionary<Type, object>> globalCache = new();
    private static Func<IList<IOptionsFactory>>? factoriesAction;
    private static readonly Dictionary<object, OptionsMapping> mappings = new();

    internal static OptionsMapping? Resolve(object mappedEntry)
    {
        if (mappings.TryGetValue(mappedEntry, out OptionsMapping mapping))
        {
            return mapping;
        }
        return null;
    }

    internal OptionsBase<T> Get<T>(object entry, T? parent = null) where T : OptionsBase<T>
    {
        return entry switch
        {
            Type type => this.Get(type, parent),
            MemberInfo memberInfo => this.Get(memberInfo, parent),
            ParameterInfo parameterInfo => this.Get(parameterInfo, parent),
            Assembly assembly => this.Get(assembly, parent),
            TypeTransferObject typeTransferObject => this.Get(typeTransferObject, parent),
            MemberTransferObject memberTransferObject => this.Get(memberTransferObject, parent),
            _ => throw new InvalidOperationException($"Entry of type {entry.GetType()} is not supported.")
        };
    }

    public static T GetGlobal<T>(MemberInfo member) where T : OptionsBase<T>
    {
        return GetOrCreateGlobal(member, GetGlobal<T>(member.DeclaringType ?? throw new InvalidOperationException()));
    }

    public T Get<T>(MemberInfo member, T? parent = null) where T : OptionsBase<T>
    {
        return this.GetOrCreate(member, parent ?? this.Get<T>(member.DeclaringType ?? throw new InvalidOperationException()), GetGlobal<T>(member));
    }

    public static T GetGlobal<T>(ParameterInfo parameter) where T : OptionsBase<T>
    {
        return GetOrCreateGlobal(parameter, GetGlobal<T>(parameter.Member));
    }

    public T Get<T>(ParameterInfo parameter, T? parent = null) where T : OptionsBase<T>
    {
        return this.GetOrCreate(parameter, parent ?? this.Get<T>(parameter.Member), GetGlobal<T>(parameter));
    }

    public static T GetGlobal<T>(Type type) where T : OptionsBase<T>
    {
        return GetOrCreateGlobal(type, GetGlobal<T>(type.Assembly));
    }

    public T Get<T>(Type type, T? parent = null) where T : OptionsBase<T>
    {
        return this.GetOrCreate(type, parent ?? this.Get<T>(type.Assembly), GetGlobal<T>(type));
    }

    public static T GetGlobal<T>(Assembly assembly) where T : OptionsBase<T>
    {
        return GetOrCreateGlobal(assembly, GetGlobal<T>());
    }

    public T Get<T>(Assembly assembly, T? parent = null) where T : OptionsBase<T>
    {
        return this.GetOrCreate(assembly, parent ?? this.Get<T>(), GetGlobal<T>(assembly));
    }

    public static T GetGlobal<T>(TypeTransferObject type) where T : OptionsBase<T>
    {
        return GetOrCreateGlobal(type, GetGlobal<T>( /*type.Assembly*/));
    }

    public T Get<T>(TypeTransferObject type, T? parent = null) where T : OptionsBase<T>
    {
        if (mappings.TryGetValue(type, out OptionsMapping mapping))
        {
            return mapping.Execute<T>(this);
        }
        return this.GetOrCreate(type, parent ?? this.Get<T>( /*type.Assembly*/), GetGlobal<T>(type));
    }

    public static T GetGlobal<T>(ITransferObject transferObject) where T : OptionsBase<T>
    {
        return GetOrCreateGlobal(transferObject, GetGlobal<T>());
    }

    public T Get<T>(ITransferObject transferObject, T? parent = null) where T : OptionsBase<T>
    {
        if (mappings.TryGetValue(transferObject, out OptionsMapping mapping))
        {
            return mapping.Execute<T>(this);
        }
        return this.GetOrCreate(transferObject, parent ?? this.Get<T>(), GetGlobal<T>(transferObject));
    }

    public static T GetGlobal<T>(MemberTransferObject member) where T : OptionsBase<T>
    {
        return GetOrCreateGlobal(member, GetGlobal<T>(member.DeclaringType ?? throw new InvalidOperationException()));
    }

    public T Get<T>(MemberTransferObject member, T? parent = null) where T : OptionsBase<T>
    {
        if (mappings.TryGetValue(member, out OptionsMapping mapping))
        {
            return mapping.Execute<T>(this);
        }
        return this.GetOrCreate(member, parent ?? this.Get<T>(member.DeclaringType ?? throw new InvalidOperationException()), GetGlobal<T>(member));
    }

    public static T GetGlobal<T>() where T : OptionsBase<T>
    {
        return GetOrCreateGlobal<T>(RootKey, null);
    }

    public T Get<T>() where T : OptionsBase<T>
    {
        return this.GetOrCreate(RootKey, null, GetGlobal<T>());
    }

    private static T GetOrCreateGlobal<T>(object key, T? parent) where T : OptionsBase<T>
    {
        key.AssertIsNotNull(nameof(key));
        if (!globalCache.TryGetValue(key, out Dictionary<Type, object> set))
        {
            set = new Dictionary<Type, object>();
            globalCache.Add(key, set);
        }
        if (set.TryGetValue(typeof(T), out object found))
        {
            return (T)found;
        }
        T entry = CreateGlobal(key, parent);
        set[typeof(T)] = entry;
        return entry;
    }

    protected T GetOrCreate<T>(object key, T? parent, T global) where T : OptionsBase<T>
    {
        key.AssertIsNotNull(nameof(key));
        if (!this.cache.TryGetValue(key, out Dictionary<Type, object> set))
        {
            set = new Dictionary<Type, object>();
            this.cache.Add(key, set);
        }
        T entry;
        if (set.TryGetValue(typeof(T), out object found))
        {
            entry = (T)found;
            if (parent != null)
            {
                entry.AddParent(parent);
            }
        }
        else
        {
            entry = Create(key, parent, global);
            set.Add(typeof(T), entry);
        }
        return entry;
    }

    private static T Create<T>(object key, T? parent, T global) where T : OptionsBase<T>
    {
        IOptionsFactory? factory = factoriesAction?.Invoke().First(x => x.CanCreate(typeof(T)));
        return (T)(factory?.Create(typeof(T), key, parent, global) ?? throw new InvalidOperationException($"Factory for {typeof(T).Name} not found"));
    }

    private static T CreateGlobal<T>(object key, T? parent) where T : OptionsBase<T>
    {
        IOptionsFactory? factory = factoriesAction?.Invoke().First(x => x.CanCreate(typeof(T)));
        return (T)(factory?.CreateGlobal(typeof(T), key, parent) ?? throw new InvalidOperationException($"Factory for {typeof(T).Name} not found"));
    }

    public static void Register(Func<IList<IOptionsFactory>> factoryListAction)
    {
        factoriesAction = factoryListAction;
    }

    public void Map<T>(object key, Expression<Func<T>> action) where T : OptionsBase<T>
    {
        if (mappings.ContainsKey(key))
        {
            return;
        }
        if (action.Body is MethodCallExpression methodCall)
        {
            MethodInfo methodInfo = methodCall.Method.GetGenericMethodDefinition();
            // object? instance = methodCall.Object == null ? null : Expression.Lambda(methodCall.Object).Compile().DynamicInvoke();
            object?[] arguments = methodCall.Arguments
                                            .Select(arg => Expression.Lambda(arg).Compile().DynamicInvoke())
                                            .ToArray();

            foreach (object? argument in arguments)
            {
                if (argument is not T options)
                {
                    continue;
                }
                object? argumentKey = this.ReverseSearch(options);
                if (argumentKey != null /*&& mappings.Values.Any(x => x.Arguments.Contains(argumentKey))*/)
                {
                    arguments.Replace(argument, argumentKey);
                    continue;
                }
                throw new InvalidOperationException($"Parameter of type {typeof(T)} are not allowed. Remove them or map it first!");
            }
            mappings.Add(key, new OptionsMapping(methodInfo, arguments));
        }
        else
        {
            throw new InvalidOperationException("The provided expression does not contain a method call.");
        }
    }

    private object? ReverseSearch<T>(OptionsBase<T> options) where T : OptionsBase<T>
    {
        foreach ((object? key, Dictionary<Type, object>? cacheValue) in this.cache.Select(x => (x.Key, x.Value)))
        {
            if (cacheValue.ContainsValue(options))
            {
                return key;
            }
        }
        return null;
    }
}
