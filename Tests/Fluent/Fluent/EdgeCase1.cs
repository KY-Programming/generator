namespace Types
{
    /// <summary>
    /// Type with Generic and non-generic base type with same name
    /// </summary>
    public class EdgeCase1 : EdgeCase1SubType<string>
    {

    }

    public class EdgeCase1SubType<T> : EdgeCase1SubType
    {
        public T GenericProperty { get; set; }
    }

    public class EdgeCase1SubType
    {
        public string Property { get; set; }
    }
}
