namespace Types
{
    public interface IInterface
    {
        public string Property { get; set; }
    }

    public interface IInterface<T>
    {
        public T Property { get; set; }
    }
}
