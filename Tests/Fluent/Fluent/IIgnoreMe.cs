namespace Types
{
    public interface IIgnoreMe
    {
        string IgnoredProperty { get; set; }
    }

    public interface IIgnoreMe<T>
    {
        T IgnoredProperty { get; set; }
    }
}
