using System.Reflection;

namespace KY.Generator.Syntax;

public class SetFluentSyntax : SetFluentSyntax<ISetFluentSyntax>, ISetFluentSyntax
{
    public SetFluentSyntax(Assembly assembly, Options options)
        : base(assembly, options)
    { }

    public SetFluentSyntax(Type type, Options options)
        : base(type, options)
    { }

    public SetFluentSyntax(MemberInfo member, Options options)
        : base(member, options)
    { }

    protected override ISetFluentSyntax GetReturn()
    {
        return this;
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

    public T PropertiesToFields()
    {
        this.GeneratorOptions.PropertiesToFields = true;
        return this.GetReturn();
    }

    public T FieldsToProperties()
    {
        this.GeneratorOptions.FieldsToProperties = true;
        return this.GetReturn();
    }

    public T PreferInterfaces()
    {
        this.GeneratorOptions.PreferInterfaces = true;
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
