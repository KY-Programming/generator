namespace SecondAssembly;

/// <summary>
/// Lives in a second assembly and carries no generate attribute of its own. It is generated anyway,
/// because <c>MainAssembly.TypeToRead</c> has a property of this type and the generator resolves and
/// follows that reference.
/// </summary>
public class SecondType
{
    public string StringProperty { get; set; } = "";
}
