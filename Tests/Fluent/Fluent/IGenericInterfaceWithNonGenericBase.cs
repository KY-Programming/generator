namespace Types
{
    public interface IGenericInterfaceWithNonGenericBase<T> : IInterface
    {
        public T GenericProperty { get; set; }
    }
}