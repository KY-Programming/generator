namespace Types
{
    public class TypeWithGenericAndNotGenericInterface : IInterface<string>, IInterface
    {
        public string Property { get; set; }
    }
}