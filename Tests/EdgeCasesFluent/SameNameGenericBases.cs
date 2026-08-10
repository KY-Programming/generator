namespace EdgeCasesFluent;

/// <summary>A generic and a non generic base type of the same name, the generic one in the middle.</summary>
public class EdgeCase1 : EdgeCase1SubType<string>;

public class EdgeCase1SubType<T> : EdgeCase1SubType
{
    public T? GenericProperty { get; set; }
}

public class EdgeCase1SubType
{
    public string Property { get; set; } = "";
}

/// <summary>The same pair the other way round: the non generic type derives from the generic one.</summary>
public class EdgeCase2 : EdgeCase2SubType;

public class EdgeCase2SubType<T>
{
    public T? GenericProperty { get; set; }
}

public class EdgeCase2SubType : EdgeCase2SubType<string>
{
    public string Property { get; set; } = "";
}
