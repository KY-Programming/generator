namespace Types
{
    public class TypeWithGenericInterface : IGenericInterface<string>
    {
        public string Property { get; set; }
    }
}
