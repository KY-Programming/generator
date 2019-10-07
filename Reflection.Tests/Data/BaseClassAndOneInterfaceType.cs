namespace KY.Generator.Reflection.Tests
{
    public class BaseClassAndOneInterfaceType : OneInterfaceType, ISecond
    {
        public string SecondProperty { get; set; }
    }
}