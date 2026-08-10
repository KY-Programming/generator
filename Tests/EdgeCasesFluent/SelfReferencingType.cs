namespace EdgeCasesFluent;

public class SelfReferencingType
{
    public string Property { get; set; } = "";
    public SelfReferencingType? Self { get; set; }
}
