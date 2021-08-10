namespace Types
{
    public class TypeWithIgnoreGenericInterface : IIgnoreMe<string>
    {
        public string IgnoredProperty { get; set; }
    }
}