namespace KY.Generator.Examples.Reflection
{
    public class ExampleType
    {
        // Types
        public string StringProperty { get; set; }
        public int IntProperty { get; set; }
        public double DoubleProperty { get; set; }
        public bool BoolProperty { get; set; }
        public SubType SubTypeProperty { get; set; }

        // Accessors
        public string ReadonlyProperty => string.Empty;
        public string WriteonlyProperty { set {} }
        protected string ProtectedProperty { get; set; }
        private string PrivateProperty { get; set; }
    }

    public class SubType
    {
        public string Property { get; set; }
    }
}