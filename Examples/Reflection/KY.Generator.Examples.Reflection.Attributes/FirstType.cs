using KY.Generator.Reflection.Attributes;

namespace KY.Generator.Examples.Reflection.Attributes
{
    [Generate]
    internal class FirstType
    {
        public string StringProperty { get; set; }
        public short ShortProperty { get; set; }
        public ushort UShortProperty { get; set; }
        public int IntProperty { get; set; }
        public uint UIntProperty { get; set; }
        public long LongProperty { get; set; }
        public ulong ULongProperty { get; set; }
        public float FloatProperty { get; set; }
        public double DoubleProperty { get; set; }
        public bool BoolProperty { get; set; }
        public object ObjectProperty { get; set; }
    }
}