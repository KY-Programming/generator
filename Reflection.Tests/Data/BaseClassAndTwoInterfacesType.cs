namespace KY.Generator.Reflection.Tests
{
    public class BaseClassAndTwoInterfacesType : OneInterfaceType, ISecond, IThird
    {
        public string SecondProperty { get; set; }
        public string ThirdProperty { get; set; }
    }
}