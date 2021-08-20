namespace Types
{
    public class TypeWithGenericAndNotGenericBaseInterface : IGenericInterfaceWithNonGenericBase<string>
    {
        public string Property { get; set; }
        public string GenericProperty { get; set; }
    }
}
