using System.Collections.Generic;

namespace KY.Generator.Templates
{
    public abstract class AttributeableTempalte : CodeFragment
    {
        public List<AttributeTemplate> Attributes { get; }

        protected AttributeableTempalte()
        {
            this.Attributes = new List<AttributeTemplate>();
        }
    }
}