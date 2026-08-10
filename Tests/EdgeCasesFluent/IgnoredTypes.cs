namespace EdgeCasesFluent;

// Base classes and interfaces that are ignored from the fluent side, plus the types deriving from them.

/// <summary>Both are ignored via SetType(...).Ignore(), so the deriving types must not inherit their member.</summary>
public class IgnoreMe
{
    public string IgnoredProperty { get; set; } = "";
}

public class IgnoreMe<T>
{
    public T? IgnoredProperty { get; set; }
}

public interface IIgnoreMe
{
    string IgnoredProperty { get; set; }
}

public interface IIgnoreMe<T>
{
    T IgnoredProperty { get; set; }
}

public class TypeWithIgnoredBase : IgnoreMe
{
    public string Property { get; set; } = "";
}

public class TypeWithGenericIgnoredBase : IgnoreMe<string>
{
    public string Property { get; set; } = "";
}

public class TypeWithIgnoreInterface : IIgnoreMe
{
    public string IgnoredProperty { get; set; } = "";
}

public class TypeWithIgnoreGenericInterface : IIgnoreMe<string>
{
    public string IgnoredProperty { get; set; } = "";
}
