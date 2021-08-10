namespace Types
{
    public class TypeWithGenericIgnoredBase : IgnoreMe<string>
    {
        public string Property { get; set; }
    }
}
