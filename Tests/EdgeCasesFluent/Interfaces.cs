namespace EdgeCasesFluent;

// A generic and a non generic interface of the same name, an interface deriving from a non generic one,
// and the types implementing them - one of them implementing both spellings at once.

public interface IInterface
{
    string Property { get; set; }
}

public interface IInterface<T>
{
    T Property { get; set; }
}

public interface IGenericInterface<T>
{
    T Property { get; set; }
}

public interface IGenericInterfaceWithNonGenericBase<T> : IInterface
{
    T GenericProperty { get; set; }
}

public class TypeWithInterface : IInterface
{
    public string Property { get; set; } = "";
}

public class TypeWithGenericInterface : IGenericInterface<string>
{
    public string Property { get; set; } = "";
}

/// <summary>Implements the generic and the non generic interface of the same name at once.</summary>
public class TypeWithGenericAndNotGenericInterface : IInterface<string>, IInterface
{
    public string Property { get; set; } = "";
}

public class TypeWithGenericAndNotGenericBaseInterface : IGenericInterfaceWithNonGenericBase<string>
{
    public string Property { get; set; } = "";
    public string GenericProperty { get; set; } = "";
}
