using System.Collections.Generic;

namespace KY.Generator.Templates
{
    public abstract class ChainedCodeFragment : CodeFragment
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

        public IEnumerable<CodeFragment> Yield()
        {
            yield return this;
            if (this.Next == null)
            {
                yield break;
            }
            foreach (CodeFragment fragment in this.Next.Yield())
            {
                yield return fragment;
            }
        }
    }
}