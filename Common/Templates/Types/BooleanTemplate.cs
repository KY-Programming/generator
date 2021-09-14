using System;

namespace KY.Generator.Templates
{
    public class BooleanTemplate : ChainedCodeFragment, ICloneable
    {
        public override string Separator => " ";
        public bool Value { get; }

        public BooleanTemplate(bool value)
        {
            this.Value = value;
        }

        public object Clone()
        {
            return new BooleanTemplate(this.Value);
        }
    }
}
