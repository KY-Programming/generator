using System.Collections.Generic;

namespace KY.Generator.Templates
{
    public abstract class ChainedCodeFragment : ICodeFragment
    {
        public abstract string Separator { get; }
        public ChainedCodeFragment Next { get; set; }
        public ChainedCodeFragment Previous { get; set; }
        public bool NewLineAfter { get; set; }

        public ChainedCodeFragment First()
        {
            return this.Previous == null ? this : this.Previous.First();
        }

        public ChainedCodeFragment Last()
        {
            return this.Next == null ? this : this.Next.Last();
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