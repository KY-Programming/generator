namespace KY.Generator;

public interface ISetFluentSyntax : ISetFluentSyntax<ISetFluentSyntax>
{
    /// <summary>
    /// Generates this type as a class, no matter which preference applies around it. Applies to this type only, not to
    /// the types generated through it. Only valid in <c>SetType</c>, the generation fails on a C# interface, on an enum
    /// and together with <see cref="ForceInterface"/>
    /// </summary>
    ISetFluentSyntax ForceClass();

    /// <summary>
    /// Generates this type as an interface, no matter which preference applies around it. Applies to this type only, not
    /// to the types generated through it. Only valid in <c>SetType</c>, the generation fails on a type with constants,
    /// on an enum and together with <see cref="ForceClass"/>
    /// </summary>
    ISetFluentSyntax ForceInterface();
}

public interface ISetFluentSyntax<out T>
{
    /// <summary>
    /// Generates interfaces instead of classes for this type and every type generated through it. The counterpart of
    /// <see cref="PreferClasses"/>
    /// </summary>
    T PreferInterfaces();

    /// <summary>
    /// Generates classes instead of interfaces for this type and every type generated through it, even when interfaces
    /// are preferred further out. The counterpart of <see cref="PreferInterfaces"/>
    /// </summary>
    T PreferClasses();

    // TODO: Document
    T OptionalFields();
    T OptionalProperties();
    T Ignore();
    T ReplaceName(string replace, string with);
    T OnlySubTypes();
    T FormatNames(bool value = true);
}
