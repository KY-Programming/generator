namespace Types
{
    /// <summary>
    /// Type with Generic and non-generic base type with same name
    /// </summary>
    public class EdgeCase2 : EdgeCase2SubType
    {

    }

    public class EdgeCase2SubType<T>
    {
        public T GenericProperty { get; set; }
    }

    public class EdgeCase2SubType : EdgeCase2SubType<string>
    {
        public string Property { get; set; }
    }
}
