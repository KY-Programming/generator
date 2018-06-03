using System.Collections.Generic;

namespace KY.Generator.Templates
{
    public class MultilineCodeFragment : ICodeFragment
    {
        public List<ICodeFragment> Fragments { get; }

        public MultilineCodeFragment()
        {
            this.Fragments = new List<ICodeFragment>();
        }

        public MultilineCodeFragment AddLine(ICodeFragment code)
        {
            this.Fragments.Add(code);
            return this;
        }

        public MultilineCodeFragment AddBlankLine()
        {
            this.Fragments.Add(Code.BlankLine());
            return this;
        }
    }
}