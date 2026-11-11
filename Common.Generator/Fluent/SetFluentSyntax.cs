using System.Reflection;

namespace KY.Generator;

public class SetFluentSyntax : SetFluentSyntax<ISetFluentSyntax>, ISetFluentSyntax
{
    private readonly Type? type;

    public SetFluentSyntax(Assembly assembly, Options options)
        : base(assembly, options)
    { }

    public SetFluentSyntax(Type type, Options options)
        : base(type, options)
    {
        this.type = type;
    }

    public SetFluentSyntax(MemberInfo member, Options options)
        : base(member, options)
    { }

    protected override ISetFluentSyntax GetReturn()
    {
        return this;
    }

    public ISetFluentSyntax ForceClass()
    {
        this.AssertIsType(nameof(this.ForceClass), nameof(this.PreferClasses));
        this.GeneratorOptions.ForceClass = true;
        return this;
    }

    public ISetFluentSyntax ForceInterface()
    {
        this.AssertIsType(nameof(this.ForceInterface), nameof(this.PreferInterfaces));
        this.GeneratorOptions.ForceInterface = true;
        return this;
    }

    /// <summary>
    /// A force applies to the one type it is set on, on an assembly it would silently do nothing
    /// </summary>
    private void AssertIsType(string method, string alternative)
    {
        if (this.type == null)
        {
            throw new InvalidOperationException($"{method}() pins a single type and is only valid in SetType<T>(). Use {alternative}() to change the preference for an assembly");
        }
    }
}

public abstract class SetFluentSyntax<T> : ISetFluentSyntax<T>
    where T : ISetFluentSyntax<T>
{
    protected Options Options { get; }
    protected GeneratorOptions GeneratorOptions { get; }

    private SetFluentSyntax(Options options)
    {
        this.Options = options;
    }

    protected SetFluentSyntax(Assembly assembly, Options options)
        : this(options)
    {
        this.GeneratorOptions = this.Options.Get<GeneratorOptions>(assembly);
    }

    protected SetFluentSyntax(Type type, Options options)
        : this(options)
    {
        this.GeneratorOptions = this.Options.Get<GeneratorOptions>(type);
    }

    protected SetFluentSyntax(MemberInfo member, Options options)
        : this(options)
    {
        this.GeneratorOptions = this.Options.Get<GeneratorOptions>(member);
    }

    protected abstract T GetReturn();

    public T PreferInterfaces()
    {
        this.GeneratorOptions.PreferInterfaces = true;
        return this.GetReturn();
    }

    public T PreferClasses()
    {
        this.GeneratorOptions.PreferInterfaces = false;
        return this.GetReturn();
    }

    public T OptionalFields()
    {
        this.GeneratorOptions.OptionalFields = true;
        return this.GetReturn();
    }

    public T OptionalProperties()
    {
        this.GeneratorOptions.OptionalProperties = true;
        return this.GetReturn();
    }

    public T Ignore()
    {
        this.GeneratorOptions.Ignore = true;
        return this.GetReturn();
    }

    public T ReplaceName(string replace, string with)
    {
        this.GeneratorOptions.AddToReplaceName(replace, with);
        return this.GetReturn();
    }

    public T OnlySubTypes()
    {
        this.GeneratorOptions.OnlySubTypes = true;
        return this.GetReturn();
    }

    public T FormatNames(bool value = true)
    {
        this.GeneratorOptions.FormatNames = value;
        return this.GetReturn();
    }
}
