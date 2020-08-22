using System;
using System.Collections.Generic;

namespace KY.Generator.Templates
{
    public abstract class ChainedCodeFragment : ICodeFragment
    {
        public virtual string Separator => this.Previous is ThisTemplate || this.Previous is AccessIndexTemplate || this.Previous is ExecuteFieldTemplate || this.Previous is ExecuteMethodTemplate || this.Previous is ExecuteGenericMethodTemplate || this.Previous is ExecutePropertyTemplate || this.Previous is LocalVariableTemplate || this.Previous is NewTemplate || this.Previous is NullValueTemplate ? "." : " ";
        public ChainedCodeFragment Next { get; set; }
        public ChainedCodeFragment Previous { get; set; }
        public bool NewLineAfter { get; set; }
        public bool CloseAfter { get; set; }
        public bool BreakAfter { get; set; }

        [Obsolete]
        public ChainedCodeFragment First()
        {
            return this.Previous == null ? this : this.Previous.First();
        }
        
        [Obsolete]
        public ChainedCodeFragment Last()
        {
            return this.Next == null ? this : this.Next.Last();
        }

        public ChainedCodeFragment Close()
        {
            this.CloseAfter = true;
            return this;
        }

        public ChainedCodeFragment BreakLine()
        {
            this.BreakAfter = true;
            return this;
        }
        
        public IEnumerable<ICodeFragment> Yield()
        {
            yield return this;
            if (this.Next == null)
            {
                yield break;
            }
            foreach (ICodeFragment fragment in this.Next.Yield())
            {
                yield return fragment;
            }
        }
    }
}